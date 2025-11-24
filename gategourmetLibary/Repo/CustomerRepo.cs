using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gategourmetLibrary.Models;
using gategourmetLibrary.Service;

namespace gategourmetLibrary.Repo
{
    public class CustomerRepo : ICustomerRepo
    {
        //returns all customers
        public List<Customer> GetAllCustomers()
        {
            return null;
        }
        //adds a new customer
        public void AddCustomer(Customer customer)
        {

        }
        //deletes a customer by ID
        public void DeleteCustomer(int customerId)
        {

        }
        //updates customer information by ID
        public void UpdateCustomer(int customerId, Customer updatedCustomer)
        {

        }
        //returns a specific customer by ID
        public Customer GetCustomer(int customerId)
        {
            return null;
        }
        //returns orders for a specific customer by ID
        public List<Order> GetCustomerOrders(int customerId)
        {
            return null;
        }
        //filters customers by name
        public List<Customer> FilterCustomersByName(string name)
        {
            return null;
        }
    }
}
