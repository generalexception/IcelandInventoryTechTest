using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Inventory.Core;

namespace Inventory.Console // great conflicting naming :-)
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonData = File.ReadAllText("Data.json");
            var data = JsonSerializer.Deserialize<List<Item>>(jsonData);

            var itemProcessor = new ItemProcessor(data);
            itemProcessor.ProcessItems();

            foreach (var item in data)
            {
                System.Console.WriteLine($"{item.Name} {item.SellIn} {item.Quality}");
            }
            System.Console.ReadKey();
        }
    }
}
