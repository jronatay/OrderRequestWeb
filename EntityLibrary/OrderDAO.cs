using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EntityLibrary
{
    public class OrderDAO
    {
        private OrderRequestEntities db = new OrderRequestEntities();



        public List<OrderProduct> Order_Product_List()
        {
            return db.OrderProducts.ToList();
        }
        
        #region Add Order to Database 

        public Order GetOrder(int OrderID)
        {
            return db.Orders.Where(order => order.Id == OrderID).First();
        }

        

        public void AddOrderItems(OrderItem OrderItems)
        {
            db.OrderItems.Add(OrderItems);
            db.SaveChanges();
        }
        
        public Order OrderConfirmation(int OrderNo)
        {
            var result = db.Orders.Where(Order => Order.Id == OrderNo).First();
            return result;
        }

        #endregion


        public List<Order> Get_Order_By_Customer_ID(int CustomerId)
        {
            var result = db.Orders.Where(orders => orders.CustomerId == CustomerId).ToList();
            return result;
        }
      
       
    }
}
