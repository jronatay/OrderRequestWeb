using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EntityLibrary
{
    public class OrderRepository
    {
        private OrderRequestEntities db = new OrderRequestEntities();
        private OrderProduct OrderProduct= new OrderProduct();
        private Order Order = new Order();
        private OrderItem OrderItem= new OrderItem();

       
        public List<OrderProduct> OrderProductList()
        {
            return db.OrderProducts.ToList();
        }
       
        #region Add Order to Database 

        public int AddOrderRequestAndReturnGeneratedID(int CustomerId)
        {
            Order.CreationDate = DateTime.Now.ToLocalTime();
            Order.CustomerId = CustomerId;
            db.Orders.Add(Order);
            db.SaveChanges();
            return Order.Id;

        }
        public void UpdateOrderNo(int OrderID)
        {
            Order = GetOrder(OrderID);
            Order.OrderNo = OrderID;
            db.SaveChanges();
        }
        
        public Order GetOrder(int OrderID)
        {
            return db.Orders.Where(order => order.Id == OrderID).First();
        }

        

        public int AddOtherProductAndReturnGeneratedID(OrderModels.OrderProductsInputModel OtherRequestProduct)
        {
            OrderProduct = PopulateOrderProductByRequestProduct(OtherRequestProduct);
            db.OrderProducts.Add(OrderProduct);
            db.SaveChanges();
           
            return OrderProduct.Id;
        }
       
      

        public OrderProduct PopulateOrderProductByRequestProduct(OrderModels.OrderProductsInputModel OtherRequestProduct)
        {
            OrderProduct.ProductName = OtherRequestProduct.ProductName;
            OrderProduct.Description = OtherRequestProduct.Description;
            return OrderProduct;
        }

        public void AddOrderItems(OrderItem OrderItems)
        {
            OrderItem = OrderItems;
            db.OrderItems.Add(OrderItem);
            db.SaveChanges();
        }
        
        public Order OrderConfirmation(int OrderNo)
        {
            var result = db.Orders.Where(Order => Order.Id == OrderNo).First();
            return result;
        }

        #endregion


        public List<Order> GetOrderByCustomer(int CustomerId)
        {
            var result = db.Orders.Where(orders => orders.CustomerId == CustomerId).ToList();
            return result;
        }
      
        public List<OrderModels.OrderProductsInputModel> GetOrderDetails(int CustomerID, int OrderID)
        {
            var result = from o in db.Orders
                    join oi in db.OrderItems
                    on o.Id equals oi.OrderId
                    join op in db.OrderProducts
                    on oi.OrderProductID equals op.Id
                    where o.CustomerId==CustomerID && o.Id==OrderID
                    select new
                    {
                        ProductName = op.ProductName,
                        Description = op.Description,
                        Quantity = oi.Quantity,
                        Id=op.Id
                        

                    };

            List<OrderModels.OrderProductsInputModel> OrderProductsOverView = new List<OrderModels.OrderProductsInputModel>();
            foreach (var Products in result)
            {
                OrderModels.OrderProductsInputModel viewmodel = new OrderModels.OrderProductsInputModel();
                viewmodel.Id = Products.Id;
                viewmodel.ProductName = Products.ProductName;
                viewmodel.Description = Products.Description;
                viewmodel.Quantity = Products.Quantity;
                OrderProductsOverView.Add(viewmodel);
            }
            return OrderProductsOverView;

        }
    }
}
