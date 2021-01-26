using GreenKartTests.Reporter;
using GreenKartTests.Utils;
using NUnit.Framework;
using System;
using System.Linq;

namespace GreenKartTests.Pages
{
    internal class GreenKartHomePage : GreenKartBasicPage
    {
        internal string[] UserEntersProducts(string itemType)
        {
            var products = TestContext.Parameters.Get(itemType);
            TestContext.Out.WriteLine($"Selected products are: {products}");

            string[] productsArray = products.Split(',').Select(a => a.Trim()).ToArray();

            //products array length must be 3
            if (productsArray.Length == 3)
            {
                Report.Info($"User has selected following products: {productsArray[0]}, {productsArray[1]}, {productsArray[2]}");
                TestContext.Out.WriteLine($"User has selected following products: {productsArray[0]}, {productsArray[1]}, {productsArray[2]}"); 
                return productsArray;
            }
            else
            {
                Report.Fail("You must select 3 products");
                TestContext.Out.WriteLine("You must select 3 products");

                return null;
            }
        }

        internal string[] AddProductsToCart(string[] productsArray)
        {
            string[] prices = new string[productsArray.Length];
            for (int i = 0; i < productsArray.Length; i++)
            {
                prices[i] = ElementExt.ReadContent(Find($"//h4[text()='{productsArray[i]}']//parent::*//p[@class = 'product-price']"));
                ElementExt.ClickOnElement(Find($"//h4[text()='{productsArray[i]}']//parent::*//div[@class = 'product-action']//button"));
            }
            Report.Info("Products are added to the cart");
            TestContext.Out.WriteLine("Products are added to the cart");
            return prices;
        }

        internal bool ProceedToCheckout(string[] productsArray)
        {
            ElementExt.ClickOnElement(Find(className: "cart-icon", timeout: 5));
            for (int i = 0; i < productsArray.Length; i++)
            {
                var carItem = ElementExt.ReadContent(Find($"(//ul[@class = 'cart-items']//p[@class = 'product-name'])[{i + 1}]"));
                if (productsArray[i] == carItem)
                {
                    Report.Pass($"Product selected by user \"{productsArray[i]}\" is the same as the product " +
                        $"displayed within a cart \"{carItem}\"");
                    TestContext.Out.WriteLine($"Product selected by user \"{productsArray[i]}\" is the same as the product " +
                        $"displayed within a cart \"{carItem}\"");
                }
                else
                {
                    Report.Fail($"Product selected by user \"{productsArray[i]}\" is NOT the same as the product " +
                        $"displayed within a cart \"{carItem}\"");
                    TestContext.Out.WriteLine($"Product selected by user \"{productsArray[i]}\" is NOT the same as the product " +
                        $"displayed within a cart \"{carItem}\"");
                    return false;
                }
            }
            ElementExt.ClickOnElement(Find("//button[text()='PROCEED TO CHECKOUT']", timeout: 7));
            return true;
        }

        internal bool ValidateProducts(string[] productsArray)
        {
            for (int i = 0; i < productsArray.Length; i++)
            {
                if (Find($"//h4[text()='{productsArray[i]}']").Displayed)
                {
                    TestContext.Out.WriteLine("The product entered is valid: " + productsArray[i]);                   
                }
                else
                {
                    TestContext.Out.WriteLine("The product entered is NOT VALID!");
                    return false;
                }
            }
            return true;
        }
    }
}
