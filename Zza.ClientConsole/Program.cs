using System;
using System.Threading.Tasks;
using Zza.ClientConsole.ZzaServices;

namespace Zza.ClientConsole
{
    public class Program
    {
        private static async Task Main()
        {
            //var client = new ZzaServiceClient("BasicHttpBinding_IZzaService");
            //var products = client.GetProducts();

            //var proxy = new ZzaServiceClient("BasicHttpBinding_IZzaService");
            //var products = await proxy.GetProductsAsync();
            //foreach (var product in products)
            //{
            //    Console.WriteLine($"{product.Id} - {product.Name}");
            //}

            var p = new ZzaProxy();
            var _products = await p.GetProductsAsync();
            foreach (var _p in _products)
            {
                Console.WriteLine(_p.Name);
            }

            Console.ReadLine();
        }
    }
}
