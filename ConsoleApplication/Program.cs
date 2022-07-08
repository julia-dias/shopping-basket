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
        public static List<string> items;
        private static readonly string[] knownCommands = { "shoppingbasket" };

        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var serviceProvider = RegisterServices();

            var itemMenu = serviceProvider.GetService<Menu>();

            await itemMenu.SeedItems();

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
                        items = inputCommands.Skip(1).ToList();

                        await itemMenu.AddToBasketAsync(items);
                    }
                    else
                    {
                        Tools.ThrowConsoleError();
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