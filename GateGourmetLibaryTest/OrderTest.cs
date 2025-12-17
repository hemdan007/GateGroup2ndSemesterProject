using gategourmetLibrary.Models;
using gategourmetLibrary.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Diagnostics;

namespace GateGourmetLibaryTest
{
    public class OrderTest
    {
        [Theory]
        [InlineData(1)]
        public void AddTest(int id)
        {
            FakeOrderRepo orderRepo = new FakeOrderRepo();
            OrderService orderService = new OrderService(orderRepo);
            Order order = new Order();
            order.ID = id;
            order.OrderMade = DateTime.Now;
            order.OrderDoneBy = DateTime.Now.AddDays(7);
            order.Recipe.Add(1,new RecipePart());
            order.Recipe[1].partName = "test";


            orderService.AddOrder(order);


            Assert.Equal(id, orderRepo.DictOrders[id].ID);
        }


    }
}
