namespace Presentation.ConsoleApp.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.DTO;
    using Application.Services.Interfaces;

    public class Menu
    {
        private readonly IItemService itemService;

        public Menu(IItemService itemService)
        {
            this.itemService = itemService;
        }

        public async Task SeedItems()
        {
            try
            {
                var items = new List<ItemDto> {
                    new ItemDto { Reference = "soup", Price = 0.65m, PriceUnit = "Unit" },
                    new ItemDto { Reference = "bread", Price = 0.80m, PriceUnit = "Unit" },
                    new ItemDto { Reference = "milk", Price = 1.30m, PriceUnit = "Unit" },
                    new ItemDto { Reference = "apples", Price = 1.00m, PriceUnit = "Bag" },
                };

                foreach (var item in items)
                {
                    var response = await this.itemService.AddItemAsync(item);

                    if (response == null)
                    {
                        DisplayError();
                    }
                }

                var seededItems = await this.itemService.GetAllItemsAsync();

                if (seededItems != null)
                {
                    DisplayResultSeedingData(seededItems.ToList());
                }
            }
            catch (Exception)
            {
                DisplayErrorSeedingData();
            }
        }

        public async Task AddToBasketAsync(List<string> items)
        {
            try
            {
                Console.WriteLine("Adding to shopping basket...\r");

                var response = await this.itemService.AddToBasketAsync(items);

                if (response is null)
                {
                    DisplayError();
                    return;
                }

                if (!response.Success)
                {
                    DisplayError(response.Message);
                    return;
                }

                Console.WriteLine("\n*** Shopping Cost ***");

                Console.WriteLine("Subtotal: €{0}", response.SubTotal);

                if (response.DiscountItems != null && response.DiscountItems.Count > 0)
                {
                    foreach (var item in response.DiscountItems)
                    {
                        Console.WriteLine("{0} {1}% off: -€{2}", item.ItemReference, item.DiscountPercentage, item.DiscountPrice);
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

        public void DisplayMenu()
        {
            Console.WriteLine("******************************\r");
            Console.WriteLine("******* SHOPPING BASKET ******\r", Console.Title);
            Console.WriteLine("******************************\n");

            Console.WriteLine("Add items to basket with the following syntax (and press Enter):\r");
            Console.WriteLine("  shoppingbasket <item1> <item2>\r");
            Console.WriteLine("\n");
        }

        public void ThrowConsoleError()
        {
            Console.Write("\r\nERROR: Command not found\r");
            Console.WriteLine("\n");
            Console.Write("\r\nPress Enter to return to Main Menu");

            Console.ReadLine();
            Console.Clear();
        }

        private static void DisplayResultSeedingData(List<ItemDto> items)
        {
            Console.Write("\r\n**** Items successfully seeded! ****");
            Console.WriteLine("\nAvailable Items:");

            foreach (var item in items)
            {
                Console.WriteLine("- {0}", item.Reference);
            }
        }

        private static void DisplayResult()
        {
            Console.Write("\r\nPress Enter to return to Main Menu");

            Console.ReadLine();

            Console.Clear();
        }

        private static void DisplayErrorSeedingData()
        {
            Console.Write("\r\nOops! Something went wrong while seeding data... Press Enter to return to Main Menu");

            Console.ReadLine();

            Console.Clear();
        }

        private static void DisplayError(string message = "")
        {
            var errorMessage = string.IsNullOrEmpty(message)
                ? "Oops! Something went wrong... Press Enter to return to Main Menu"
                : message;

            Console.Write("\r\n{0}", message);

            Console.ReadLine();

            Console.Clear();
        }
    }
}