using gategourmetLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gategourmetLibrary.Repo
{
    interface IOrderRepo 
    {
        void Add(Order order);
        Order Get(int orderID);
        void Delete(int orderPrimaryKey);
        Dictionary<int, Order> GetAll();
        void Update(int orderID, Order updateOrder);

    }
}
