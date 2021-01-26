using GreenKartTests.Reporter;
using GreenKartTests.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace GreenKartTests.Pages
{
    class GreenKartCheckOutPage : GreenKartBasicPage
    {
        const string promoCode = "rahulshettyacademy";
        private IWebElement PromoCodeField => Find(className: "promoCode", timeout: 5);
        private IWebElement ApplyPromoCodeBtn => Find(className: "promoBtn", timeout: 5);
        private IWebElement PromoInfo => Find(className: "promoInfo", timeout: 15);
        private IWebElement TotalAmount => Find(className: "totAmt");
        private IWebElement TotalAfterDiscount => Find(className: "discountAmt");
        private IWebElement DiscountPercentage => Find(className: "discountPerc");
        private IWebElement PlaceOrderBtn => Find("//button[text()='Place Order']");

        internal bool ComparePrices(string[] productsPrice, string[] productsNameArray)
        {
            string[] checkOutproductPrices = ReadCheckoutProductsPrice(productsNameArray);

            for (int i = 0; i < productsPrice.Length; i++)
            {
                if (Array.Exists(productsPrice, element => element == checkOutproductPrices[i]))
                {
                    Report.Pass($"Product price, from the selected product \"{productsNameArray[i]}\", equals \"{productsPrice[i]}\" and it's the same as the price" +
                        $" within the cart: \"{checkOutproductPrices[i]}\" for the same product");
                    TestContext.Out.WriteLine($"Product price, from the selected product \"{productsNameArray[i]}\", equals \"{productsPrice[i]}\" and it's the same as the price" +
                        $" within the cart: \"{checkOutproductPrices[i]}\" for the same product");
                }
                else
                {
                    Report.Fail($"Product price, from the selected product \"{productsNameArray[i]}\", equals \"{productsPrice[i]}\" and IT IS NOT the same as the price" +
                        $" within the cart: \"{checkOutproductPrices[i]}\" for the same product");
                    TestContext.Out.WriteLine($"Product price, from the selected product \"{productsNameArray[i]}\", equals \"{productsPrice[i]}\" and IT IS NOT the same as the price" +
                        $" within the cart: \"{checkOutproductPrices[i]}\" for the same product");
                    return false;
                }
            }
            return true;         
        }
        internal string[] ReadCheckoutProductsPrice(string[] productsArray)
        {
            string[] checkOutproductsPrice = new string[productsArray.Length];

            for (int i = 0; i < productsArray.Length; i++)
            {
                checkOutproductsPrice[i] = ElementExt.ReadContent(Find($"(//p[text()= '{productsArray[i]}']//parent::*//parent::*//p[@class = 'amount'])[1]", timeout: 10));
            }
            return checkOutproductsPrice;
        }
        internal void ApplyPromoCode()
        {
            PromoCodeField.SendKeys(promoCode);
            Report.Info($"\"{promoCode}\" promo code value is populated");
            ElementExt.ClickOnElement(ApplyPromoCodeBtn);
        }
        internal bool VerifyPromoCodeApplied()
        {
            string successMsg = "Code applied ..!";
            string promoInfo = ElementExt.ReadContent(PromoInfo);
            if (promoInfo == successMsg)
            {
                Report.Pass("Promo Code is successfully applied!");
                TestContext.Out.WriteLine("Promo Code is successfully applied!");
                return true;
            }
            else
            {
                Report.Fail("Promo code has failed to apply!");
                TestContext.Out.WriteLine("Promo code has failed to apply!");
                return false;
            }
        }
        internal bool VerifyPriceIsDiscounted()
        {
            string totalAmount = ElementExt.ReadContent(TotalAmount);
            Int32.TryParse(totalAmount, out int numValue);
            string discountedAmount = ElementExt.ReadContent(TotalAfterDiscount);
            Int32.TryParse(discountedAmount, out int discNumValue);
            string discountPercent = ElementExt.ReadContent(DiscountPercentage);
            if (numValue > discNumValue)
            {
                Report.Pass($"Initial total price {totalAmount} is discounted by {discountPercent} and now results: {discountedAmount}");
                TestContext.Out.WriteLine($"Initial total price {totalAmount} is discounted by {discountPercent} and now results: {discountedAmount}");
                return true;
            }
            else
            {
                Report.Fail($"Initial total price {totalAmount} is NOT discounted by {discountPercent} and now results: {discountedAmount}");
                TestContext.Out.WriteLine($"Initial total price {totalAmount} is NOT discounted by {discountPercent} and now results: {discountedAmount}");

                return false;
            }
        }
        internal void PlaceOrder()
        {
            ElementExt.ClickOnElement(PlaceOrderBtn);
            Report.Info("Proceed to country selection page by clicking on the \"Place order\" button");
        }
    }
}
