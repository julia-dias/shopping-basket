namespace Presentation.ConsoleApp.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Services.Interfaces;

    public class Options
    {
        private readonly IShoppingBasketService shoppingBasketService;

        public Options(IShoppingBasketService shoppingBasketService)
        {
            this.shoppingBasketService = shoppingBasketService;
        }

        public async Task AddToBasketAsync(List<string> items)
        {
            try
            {
                Console.WriteLine("Adding to shopping basket...\r");

                var response = await this.shoppingBasketService.AddToBasketAsync(items);

                if (response == null || !response.Success)
                {
                    DisplayError();
                }

                Console.WriteLine("\n*** Shopping Cost ***");

                Console.WriteLine("Subtotal: €{0}", response.SubTotal);

                if (response.DiscountItems != null && response.DiscountItems.Count > 0)
                {
                    foreach (var item in response.DiscountItems)
                    {
                        Console.WriteLine("{0} {1}% off: -€{2}", item.ItemName, item.DiscountPercentage, item.DiscountPrice);
                    }
                }
                else
                {
                    Console.WriteLine("(No offers available)");
                }

                Console.WriteLine("Total price: €{0}", response.Total);

                DisplayResult();
            }
            catch (Exception)
            {
                DisplayError();
            }
        }

        private static void DisplayResult()
        {
            Console.Write("\r\nPress Enter to return to Main Menu");

            Console.ReadLine();

            Console.Clear();
        }

        private static void DisplayError()
        {
            Console.Write("\r\nOops! Something went wrong... Press Enter to return to Main Menu");

            Console.ReadLine();

            Console.Clear();
        }
    }
}