namespace Presentation.ConsoleApp.Helpers
{
    using System;

    public class Tools
    {
        public static void DisplayMenu()
        {
            Console.WriteLine("******************************\r");
            Console.WriteLine("******* SHOPPING BASKET ******\r", Console.Title);
            Console.WriteLine("******************************\n");

            Console.WriteLine("Add items to basket with the following syntax (and press Enter):\r");
            Console.WriteLine("  shoppingbasket <item1> <item2>\r");
            Console.WriteLine("\n");
        }

        public static void ThrowConsoleError()
        {
            Console.Write("\r\nERROR: Command not found\r");
            Console.WriteLine("\n");
            Console.Write("\r\nPress Enter to return to Main Menu");

            Console.ReadLine();
            Console.Clear();
        }
    }
}