using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Zza.Entities;
using Zza.Client.ZzaServices;

namespace Zza.Client
{
    public class MainWindowViewModel : BindableBase
    {
        private ObservableCollection<Product> _Products;
        private ObservableCollection<Customer> _Customers;
        private ObservableCollection<OrderItemModel> _Items = new ObservableCollection<OrderItemModel>();
        private Order _CurrentOrder = new Order();

        public MainWindowViewModel()
        {
            _CurrentOrder.OrderDate = DateTime.Now;
            _CurrentOrder.OrderStatusId = 1;
            SubmitOrderCommand = new DelegateCommand(OnSubmitOrder);
            AddOrderItemCommand = new DelegateCommand<Product>(OnAddItem);
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                LoadProductsAndCustomers();
            }
        }

        public ObservableCollection<Product> Products
        {
            get => _Products;
            set => SetProperty(ref _Products, value);
        }

        public ObservableCollection<Customer> Customers
        {
            get => _Customers;
            set => SetProperty(ref _Customers, value);
        }

        public ObservableCollection<OrderItemModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }

        public Order CurrentOrder
        {
            get { return _CurrentOrder; }
            set { SetProperty(ref _CurrentOrder, value); }
        }

        public DelegateCommand SubmitOrderCommand { get; private set; }
        public DelegateCommand<Product> AddOrderItemCommand { get; private set; }

        private void OnAddItem(Product product)
        {
            var existingOrderItem = _CurrentOrder.OrderItems.Where(oi => oi.ProductId == product.Id).FirstOrDefault();
            var existingOrderItemModel = _Items.Where(i => i.ProductId == product.Id).FirstOrDefault();
            if (existingOrderItem != null && existingOrderItemModel != null)
            {
                existingOrderItem.Quantity++;
                existingOrderItemModel.Quantity++;
                existingOrderItem.TotalPrice = existingOrderItem.UnitPrice * existingOrderItem.Quantity;
                existingOrderItemModel.TotalPrice = existingOrderItem.TotalPrice;
            }
            else
            {
                var orderItem = new OrderItem { ProductId = product.Id, Quantity = 1, UnitPrice = 9.99m, TotalPrice = 9.99m };
                _CurrentOrder.OrderItems.Add(orderItem);
                Items.Add(new OrderItemModel { ProductId = product.Id, ProductName = product.Name, Quantity = orderItem.Quantity, TotalPrice = orderItem.TotalPrice });
            }

        }

        private void LoadProductsAndCustomers()
        {
            ZzaProxy proxy = new ZzaProxy("NetTcpBinding_IZzaService");
            //ZzaServiceClient proxy = new ZzaServiceClient("NetTcpBinding_IZzaService");
            try
            {
                Products = proxy.GetProducts();
                Customers = proxy.GetCustomers();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load server data. " + ex.Message);
            }
            finally
            {
                proxy.Close();
            }

        }

        private void OnSubmitOrder()
        {
            if (_CurrentOrder.CustomerId != Guid.Empty && _CurrentOrder.OrderItems.Count > 0)
            {
                ZzaServiceClient proxy = new ZzaServiceClient("NetTcpBinding_IZzaService");
                try
                {
                    proxy.SubmitOrder(_CurrentOrder);
                    CurrentOrder = new Order();
                    CurrentOrder.OrderDate = DateTime.Now;
                    CurrentOrder.OrderStatusId = 1;
                    Items = new ObservableCollection<OrderItemModel>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving order, please try again later.");
                    // Log it
                }
                finally
                {
                    proxy.Close();
                }
            }
            else
            {
                MessageBox.Show("You must select a customer and add order items to submit an order");
            }
        }
    }
    public class OrderItemModel : BindableBase
    {
        private int _ProductId;

        public int ProductId
        {
            get { return _ProductId; }
            set { SetProperty(ref _ProductId, value); }
        }

        private string _ProductName;

        public string ProductName
        {
            get { return _ProductName; }
            set { SetProperty(ref _ProductName, value); }
        }

        private int _Quantity;

        public int Quantity
        {
            get { return _Quantity; }
            set { SetProperty(ref _Quantity, value); }
        }

        private decimal _TotalPrice;

        public decimal TotalPrice
        {
            get { return _TotalPrice; }
            set { SetProperty(ref _TotalPrice, value); }
        }
    }
    class ZzaProxy : ClientBase<IZzaService>, IZzaService
    {
        public ZzaProxy() { }
        public ZzaProxy(string endpointName) : base(endpointName) { }
        public ZzaProxy(Binding binding, string address) : base(binding, new EndpointAddress(address)) { }

        public ObservableCollection<Entities.Product> GetProducts()
        {
            return Channel.GetProducts();
        }

        public Task<ObservableCollection<Entities.Product>> GetProductsAsync()
        {
            return Channel.GetProductsAsync();
        }

        public ObservableCollection<Entities.Customer> GetCustomers()
        {
            return Channel.GetCustomers();
        }

        public Task<ObservableCollection<Entities.Customer>> GetCustomersAsync()
        {
            return Channel.GetCustomersAsync();
        }

        public void SubmitOrder(Entities.Order order)
        {
            Channel.SubmitOrder(order);
        }

        public Task SubmitOrderAsync(Entities.Order order)
        {
            return Channel.SubmitOrderAsync(order);
        }
    }
}
