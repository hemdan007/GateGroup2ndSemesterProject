using Microsoft.AspNetCore.Mvc.RazorPages;
using gategourmetLibrary.Models;
using gategourmetLibrary.Service;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CompanyWebpages.Pages
{
    public class OrderTrackingModel : PageModel
    {
        private readonly DepartmentService _departmentService;
        private readonly OrderService _orderService;

        public OrderTrackingModel(DepartmentService departmentService, OrderService orderService)
        {
            _departmentService = departmentService;
            _orderService = orderService;
        }

        public List<OrderItem> OrderItems { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public string ErrorMessage { get; set; }

        public IActionResult OnGet(int orderId)
        {
            OrderId = orderId;
            
            // Hent ordre information
            try
            {
                Order = _orderService.GetOrder(orderId);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error retrieving order {orderId}: {ex.Message}";
                Order = null;
                OrderItems = new List<OrderItem>();
                return Page();
            }
            
            if (Order == null)
            {
                ErrorMessage = $"Order #{orderId} was not found in the system.";
                OrderItems = new List<OrderItem>();
                return Page();
            }

            // Hent order items
            try
            {
                OrderItems = _departmentService.GetOrderStockLocations(orderId);
                
                if (OrderItems == null)
                {
                    OrderItems = new List<OrderItem>();
                }

                if (OrderItems.Count == 0)
                {
                    ErrorMessage = $"Information: No items found for order {orderId}. The order may not have any associated items yet.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error retrieving inventory locations:{ex.Message}";
                OrderItems = new List<OrderItem>();
            }

            return Page();
        }
    }
}
