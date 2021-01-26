using GreenKartTests.Pages;
using GreenKartTests.Reporter;
using GreenKartTests.Utils;
using NUnit.Framework;

namespace GreenKartTests.Tests
{
    public class PurchaseItemsWorkFlowTest : BasicTests
    {
        [Test]
        public void PurchaseSelectedProductsTest()
        {
            GreenKartCheckOutPage greenKartCheckOut = new GreenKartCheckOutPage();
            GreenKartHomePage greenKartHomePage = new GreenKartHomePage();
            GreenKartPlaceOrderPage greenKartPlaceOrderPage = new GreenKartPlaceOrderPage();

            Report.AddNewTest("Purchase_Items_WorkFlow_Test");
            //OpenWebsite and validate if it's loaded successfully
            greenKartHomePage.OpenWebsite();
            Assert.True(VerifyWebsiteIsLoaded());

            //Get products from console and validate is number or name of products is correct
            var productsArray = greenKartHomePage.UserEntersProducts("Products");
            Assert.NotNull(productsArray);
            bool validProduct = greenKartHomePage.ValidateProducts(productsArray);
            Assert.IsTrue(validProduct);

            //Proceed to checkout and verify if the selected products are the same as in the cart
            var productsPrice = greenKartHomePage.AddProductsToCart(productsArray);
            bool validProductCart = greenKartHomePage.ProceedToCheckout(productsArray);
            Assert.IsTrue(validProductCart);

            //Checkout page
            //Comparing the prices from selected products to the ones displayed in a cart
            bool validPrices = greenKartCheckOut.ComparePrices(productsPrice, productsArray);
            Assert.IsTrue(validPrices);

            //Apply promo code and verify if it's successfully processed
            greenKartCheckOut.ApplyPromoCode();
            bool validPromoCode = greenKartCheckOut.VerifyPromoCodeApplied();
            Assert.IsTrue(validPromoCode);
            //Verify if the price is discounted and by which percent amount
            bool validDiscount = greenKartCheckOut.VerifyPriceIsDiscounted();
            Assert.IsTrue(validDiscount);
            //Continue to place order page
            greenKartCheckOut.PlaceOrder();

            //Place the order and complete the purchase
            greenKartPlaceOrderPage.SelectCountry();
            greenKartPlaceOrderPage.AcceptTermsAndConditions();
            bool completed = greenKartPlaceOrderPage.CompletePurchase();
            Assert.IsTrue(completed);
        }
    }
}
