using System;
using System.Threading.Tasks;

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
            if (p.ClientCredentials != null)
            {
                p.ClientCredentials.Windows.ClientCredential.UserName = "XPS9100\\test";
                p.ClientCredentials.Windows.ClientCredential.Password = "2644";
            }

            var _products = await p.GetProductsAsync();
            foreach (var _p in _products)
            {
                Console.WriteLine(_p.Name);
            }

            Console.ReadLine();
        }
    }
}
