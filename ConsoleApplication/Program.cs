namespace ConsoleApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Services.Implementations;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Presentation.ConsoleApp.Helpers;

    public static class Program
    {
        public static List<string> items;
        private static readonly string[] knownCommands = { "shoppingbasket" };

        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => { services.AddTransient<ShoppingBasketService>(); })
                .Build();

            var shoppingBasketService = host.Services.GetRequiredService<ShoppingBasketService>();

            bool exit = false;

            while (!exit)
            {
                Tools.DisplayMenu();

                var input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    var inputCommands = input.Split(' ');

                    if (knownCommands.Contains(inputCommands[0]) && inputCommands.Length > 1)
                    {
                        items = inputCommands.ToList();

                        var options = new Options(shoppingBasketService);

                        await options.AddToBasketAsync(items);
                    }
                    else
                    {
                        Tools.ThrowConsoleError();
                    }
                }
            }
        }
    }
}