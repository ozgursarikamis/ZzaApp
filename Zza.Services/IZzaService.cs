using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Zza.Data;
using Zza.Entities;

namespace Zza.Services
{
    [ServiceContract]
    public interface IZzaService
    {
        [OperationContract] List<Product> GetProducts();
        [OperationContract] List<Customer> GetCustomers();
        [OperationContract] void SubmitOrder(Order order);
    }

    public class ZzaService : IZzaService, IDisposable
    {
        private readonly ZzaDbContext _context = new ZzaDbContext();
        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public List<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        public void SubmitOrder(Order order)
        {
            _context.Orders.Add(order);
            order.OrderItems.ForEach(oi => _context.OrderItems.Add(oi));
            _context.SaveChanges();
        }

        public void Dispose()
        {
            // WCF uses this for us:
            _context?.Dispose();
        }
    }
}
