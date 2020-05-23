using System;
using System.ServiceModel;
using Zza.Services;

namespace Zza.SelfHost
{
    internal class Program
    {
        private static void Main()
        {
            var host = new ServiceHost(typeof(ZzaService));
            host.Open();
            Console.WriteLine("Hit any key to exit");
            Console.ReadKey();

            host.Close();
        }
    }
}
