using System;
using System.Collections.Generic;
using gategourmetLibrary.Models;
using gategourmetLibrary.Repo;
using gategourmetLibrary.Secret;
using gategourmetLibrary.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CompanyWebpages.Pages
{
    // admin page that shows all orders and allows cancelling them
    public class AdminOrderListModel : PageModel
    {
        // list with all orders to show in the table
        public List<Order> Orders { get; set; }

        // status message shown after cancel
        public string StatusMessage { get; set; }

        // error message if something goes wrong
        public string ErrorMessage { get; set; }

        // list of customers for the filter dropdown
        public List<Customer> Customers { get; set; }

        // selected customer ID for filtering (from query string)
        [BindProperty(SupportsGet = true)]
        public int? SelectedCustomerId { get; set; }

        // service that handles order logic
        private readonly OrderService _orderService;
        
        // service that handles customer logic
        private readonly CustomerService _customerService;

        // constructor creates repository and service
        public AdminOrderListModel()
        {
            string connectionString = new Connect().cstring;
            IOrderRepo orderRepo = new OrderRepo(connectionString);
            ICustomerRepo customerRepo = new CustomerRepo(connectionString);
            _orderService = new OrderService(orderRepo);
            _customerService = new CustomerService(customerRepo);
        }

        // hj�lper method that loads only non cancelled orders
        private void LoadAllOrders()
        {
            try
            {
                if (SelectedCustomerId.HasValue && SelectedCustomerId.Value > 0)
                {
                    // Filter by selected customer
                    Customer selectedCustomer = _customerService.GetCustomer(SelectedCustomerId.Value);
                    if (selectedCustomer != null)
                    {
                        Orders = _orderService.FilterOrdersByCompany(selectedCustomer);
                    }
                    else
                    {
                        Orders = new List<Order>();
                    }
                }
                else
                {
                    // Load all orders
                    Orders = _orderService.GetAllOrders();
                }
                
                if (Orders == null)
                {
                    Orders = new List<Order>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Fejl ved indlæsning af ordrer: " + ex.Message;
                Orders = new List<Order>();
            }
        }

        // loads all customers for the dropdown
        private void LoadCustomers()
        {
            try
            {
                Customers = _customerService.GetAllCustomers();
                if (Customers == null)
                {
                    Customers = new List<Customer>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Fejl ved indlæsning af kunder: " + ex.Message;
                Customers = new List<Customer>();
            }
        }

        // runs when the page is loaded with a get metode 
        public void OnGet()
        {
            LoadCustomers();
            LoadAllOrders();
        }

        // runs when the cancel form is posted
        public IActionResult OnPostCancel(int orderId)
        {
            // call service to cancel the order
            _orderService.CancelOrder(orderId);

            // set status message so admin can see what happened
            StatusMessage = "Ordre #" + orderId + " er blevet annulleret.";

            // after a POST we redirect to GET so the page reloads clean
            return RedirectToPage();
        }
    }
}
