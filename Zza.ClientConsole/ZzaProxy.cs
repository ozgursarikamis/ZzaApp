using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Zza.ClientConsole.ZzaServices;
using Zza.Entities;

namespace Zza.ClientConsole
{
    public class ZzaProxy : ClientBase<IZzaService>, IZzaService
    {
        public ZzaProxy() { }
        public ZzaProxy(string endpointName): base(endpointName) { }
        public ZzaProxy(Binding binding, string address): base(binding, new EndpointAddress(address)) { }

        public Product[] GetProducts()
        {
            return Channel.GetProducts();
        }

        public Task<Product[]> GetProductsAsync()
        {
            return Channel.GetProductsAsync();
        }

        public Customer[] GetCustomers()
        {
            return Channel.GetCustomers();
        }

        public Task<Customer[]> GetCustomersAsync()
        {
            return Channel.GetCustomersAsync();
        }

        public void SubmitOrder(Order order)
        {
            Channel.SubmitOrder(order);
        }

        public Task SubmitOrderAsync(Order order)
        {
            return Channel.SubmitOrderAsync(order);
        }
    }
}