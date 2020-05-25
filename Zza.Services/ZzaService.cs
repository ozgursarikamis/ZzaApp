using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using Zza.Data;
using Zza.Entities;

namespace Zza.Services
{
    public class ZzaService : IZzaService, IDisposable
    {
        private readonly ZzaDbContext _context = new ZzaDbContext();

        [PrincipalPermission(SecurityAction.Demand, Role = "BUILTIN\\Users")]
        public List<Product> GetProducts()
        {
            var principal = Thread.CurrentPrincipal;
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