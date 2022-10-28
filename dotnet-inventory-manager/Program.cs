using System;

namespace dotnet_inventory_manager
{
    internal class Program
    {
        private static InventoryManager? _im;
        public static void Main(string[] args)
        {

            // Check if improper usage
            if (args.Length != 2)
            {
                Console.Error.WriteLine("usage: ./main CLI inventory.csv");
                return;
            }

            var cli = args[0] switch
            {
                "1" => true,
                _ => false
            };

            _im = new InventoryManager(cli, args[1]);

            try
            {
                _im.ReadCsvFile();
                _im.UserLogin();
                while (_im.UserInput() == 0)
                {
                }
                
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return;
            }
            
        }
    }
}