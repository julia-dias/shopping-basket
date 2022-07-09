namespace ConsoleApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Services.Implementations;
    using Application.Services.Interfaces;
    using Data.Repository.Implementations;
    using Data.Repository.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Presentation.ConsoleApp.Helpers;

    public static class Program
    {
        private static readonly string[] knownCommands = { "shoppingbasket" };

        public static List<string> items;
        private static Menu menu;

        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var serviceProvider = RegisterServices();

            menu = serviceProvider.GetService<Menu>();
            await menu.SeedItems();

            bool exit = false;

            while (!exit)
            {
                menu.DisplayMenu();

                var input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    var inputCommands = input.Split(' ');

                    if (knownCommands.Contains(inputCommands[0]) && inputCommands.Length > 1)
                    {
                        items = inputCommands.Skip(1).ToList();

                        await menu.AddToBasketAsync(items);
                    }
                    else
                    {
                        menu.ThrowConsoleError();
                    }
                }
            }
        }

        private static IServiceProvider RegisterServices()
        {
            var services = new ServiceCollection()
               .AddScoped<IItemRepository, ItemRepository>()
               .AddScoped<IItemService, ItemService>()
               .AddTransient<Menu>();

            return services.BuildServiceProvider();
        }
    }
}