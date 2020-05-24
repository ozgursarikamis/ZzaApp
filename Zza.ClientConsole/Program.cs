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

            var proxy = new ZzaServiceClient("BasicHttpBinding_IZzaService");
            var products = await proxy.GetProductsAsync();
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id} - {product.Name}");
            }

            Console.ReadLine();
        }
    }
}
