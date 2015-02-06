using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EntityLibrary;
using System.Text.RegularExpressions;


namespace BusinessLogicLayer
{
    public class OrderService
    {
        private OrderRequestEntities db = new OrderRequestEntities();
        private EntityLibrary.OrderDAO OrderDAO = new OrderDAO();
        private OrderProduct OrderProduct = new OrderProduct();
        private EntityLibrary.Order Order = new EntityLibrary.Order();
        private EntityLibrary.OrderItem OrderItem = new OrderItem();
        private EntityLibrary.OrderModels.OrderProductsInputModel OrderProductInputModel = new EntityLibrary.OrderModels.OrderProductsInputModel();
        
        
        
        public object get_and_set_product_data(int Id)
        {
            OrderProductInputModel = get_order_product_and_populate_input_model(Id);
            return new { Id=OrderProductInputModel.Id,
                         ProductName=OrderProductInputModel.ProductName,
                         Description = OrderProductInputModel.Description};
        }

        public EntityLibrary.OrderModels.OrderProductsInputModel get_order_product_and_populate_input_model(int Id)
        {
            var result = db.OrderProducts.Where(product => product.Id == Id).First();

            OrderProductInputModel.Id = result.Id;
            OrderProductInputModel.ProductName = result.ProductName;
            OrderProductInputModel.Description = result.Description;
            return OrderProductInputModel;
        }

        public List<EntityLibrary.OrderModels.OrderProductsInputModel> Populated_Order_Product_From_Request(EntityLibrary.OrderModels.OrderRequestInputModel OrderInputRequest)
        {
           List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderInput = new List<EntityLibrary.OrderModels.OrderProductsInputModel>();
            if (OrderInputRequest.Id != null)
            {
                for (int counter = 0; counter < OrderInputRequest.Id.Count(); counter++)
                {

                    if (OrderInputRequest.ProductName[counter] != null && OrderInputRequest.ProductName[counter].Trim() != "" &&
                        OrderInputRequest.Description[counter] != null && OrderInputRequest.Description[counter].Trim() != "")
                    {
                        OrderProductInputModel = new EntityLibrary.OrderModels.OrderProductsInputModel();
                        OrderProductInputModel.Id = OrderInputRequest.Id[counter];
                        OrderProductInputModel.ProductName = OrderInputRequest.ProductName[counter];
                        OrderProductInputModel.Description = OrderInputRequest.Description[counter];
                        OrderProductInputModel.Quantity = OrderInputRequest.Quantity[counter];
                        OrderInput.Add(OrderProductInputModel);
                    }

                }
                return OrderInput;
            }
            return OrderInput;
        }

        public bool Is_There_At_least_One_Order(List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsInput)
        {
            foreach (var Item in OrderProductsInput)
            {
                if (Item.Quantity > 0)
                {
                   return true;
                 }
            }
            return false;
        }
        
        public bool Is_Other_Product_Data_Valid(List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsInput)
        {
            string Product_Name_and_Description_pattern = @"^[^`~!@#\$%\^&\*\(\)_\+=<>\?;:'\|/\[\]\{\}""]+[^`~!@#\$%\^&\*\(\)_\+=<>\?;:\|/\[\]\{\}""]$";
            Regex Product_Name_and_Description_regex = new Regex(Product_Name_and_Description_pattern);
            foreach (var Item in OrderProductsInput)
            {
                if (Item.Id == 0)
                {
                    if (Item.ProductName.Length > 50 || Is_Product_Name_Exist(Item.ProductName))
                    {
                        return false;
                    }
                    if (!Product_Name_and_Description_regex.IsMatch(Item.ProductName) || !Product_Name_and_Description_regex.IsMatch(Item.Description) )
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        public bool Is_Product_Name_Exist(string ProductName)
        {
            var result = db.OrderProducts.Where(order_product => order_product.ProductName == ProductName).ToList();

            return result.Count > 0;
        }

        public List<EntityLibrary.OrderModels.OrderProductsInputModel> ReturnStoredOrderProducts(List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsInput)
        {
            if (Is_There_At_least_One_Order(OrderProductsInput) )
            {
                return StoreProductsOnTemporaryStorage(OrderProductsInput);
            }
            throw  new Exception();
        }

        public List<EntityLibrary.OrderModels.OrderProductsInputModel> StoreProductsOnTemporaryStorage(List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsRequestedInput)
        {
            List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsTemporaryStorage = new List<EntityLibrary.OrderModels.OrderProductsInputModel>();
            foreach (var Item in OrderProductsRequestedInput)
            {
                if (Item.Quantity > 0 )
                {
                    OrderProductsTemporaryStorage.Add(Item);
                }
            }
            return OrderProductsTemporaryStorage;
        }

        public EntityLibrary.OrderModels.OrderProductsInputModel PopulateTemporaryStorage(EntityLibrary.OrderModels.OrderProductsInputModel OrderProductsRequestedInput)
        {
            OrderProductInputModel.Id = OrderProductsRequestedInput.Id;
            OrderProductInputModel.ProductName = OrderProductsRequestedInput.ProductName;
            OrderProductInputModel.Description = OrderProductsRequestedInput.Description;
            OrderProductInputModel.Quantity = OrderProductsRequestedInput.Quantity;
            return OrderProductInputModel;
        }

        public bool Is_Requested_Stored_In_Temporary_Storage(List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsInput)
        {
            return ReturnStoredOrderProducts(OrderProductsInput) != null;
        }

        public List<EntityLibrary.OrderModels.OrderProductsInputModel> ReturnOrderProductsStored(List<EntityLibrary.OrderModels.OrderProductsInputModel> StoredOrderProducts)
        {
            if (Is_Requested_Stored_In_Temporary_Storage(StoredOrderProducts))
            {
                return ReturnStoredOrderProducts(StoredOrderProducts);
            }
            return null;
        }

        public int New_Order_Id_and_update_order_number(int CustomerID)
        {
            Order.CreationDate = DateTime.Now.ToLocalTime();
            Order.CustomerId = CustomerID;
            db.Orders.Add(Order);
            db.SaveChanges();
            int OrderNo = Order.Id;

            Update_Order_No(OrderNo);
            return OrderNo;
        }
        
        public void Update_Order_No(int OrderID)
        {
            Order = OrderDAO.GetOrder(OrderID);
            Order.OrderNo = OrderID;
            db.SaveChanges();
        }
       
        #region OrderItems

       
        public void SaveOrder(EntityLibrary.OrderModels.OrderRequestInputModel ConfirmedOrderRequest,int OrderNo)
        {
            List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderInput = new List<EntityLibrary.OrderModels.OrderProductsInputModel>();
            OrderInput = Populated_Order_Product_From_Request(ConfirmedOrderRequest);
                foreach (var Product in OrderInput)
                {
                    if (Product.Id==0  && Product.Quantity != 0)
                    {
                        int OrderProductID = Add_Other_Product_And_Return_Generated_ID(Product);
                        OrderDAO.AddOrderItems(Populate_Order_Items_Other_Products(Product, OrderNo, OrderProductID));
                    }
                    else if (Product.Id != 0 && Product.Quantity != 0)
                    {
                        OrderDAO.AddOrderItems(Populate_Order_Items(Product, OrderNo));
                    }
                }
        }

        public int Add_Other_Product_And_Return_Generated_ID(EntityLibrary.OrderModels.OrderProductsInputModel OtherRequestProduct)
        {
            OrderProduct.ProductName = OtherRequestProduct.ProductName;
            OrderProduct.Description = OtherRequestProduct.Description;
            db.OrderProducts.Add(OrderProduct);
            db.SaveChanges();

            return OrderProduct.Id;
        }
      
        public OrderItem Populate_Order_Items(EntityLibrary.OrderModels.OrderProductsInputModel OrderProducts,int OrderNo)
        {
           
            OrderItem.Quantity = OrderProducts.Quantity;
            OrderItem.OrderProductID = OrderProducts.Id;
            OrderItem.OrderId = OrderNo;
            return OrderItem;
        }

        public OrderItem Populate_Order_Items_Other_Products(EntityLibrary.OrderModels.OrderProductsInputModel OrderProducts, int OrderNo, int OrderProductID)
        {
           
            OrderItem.Quantity = OrderProducts.Quantity;
            OrderItem.OrderProductID = OrderProductID;
            OrderItem.OrderId = OrderNo;
            return OrderItem;
        }
        
       
        
        #endregion
         
       public List<EntityLibrary.OrderModels.OrderProductsInputModel> GetOrderProductsByOrderIdAndCustomerId(int OrderId,int CustomerId)
        {
            var result = from o in db.Orders
                         join oi in db.OrderItems
                         on o.Id equals oi.OrderId
                         join op in db.OrderProducts
                         on oi.OrderProductID equals op.Id
                         where o.CustomerId == CustomerId && o.Id == OrderId
                         select new
                         {
                             ProductName = op.ProductName,
                             Description = op.Description,
                             Quantity = oi.Quantity,
                             Id = op.Id


                         };

            List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsOverView = new List<EntityLibrary.OrderModels.OrderProductsInputModel>();
            foreach (var Products in result)
            {
               OrderProductInputModel = new EntityLibrary.OrderModels.OrderProductsInputModel();
                OrderProductInputModel.Id = Products.Id;
                OrderProductInputModel.ProductName = Products.ProductName;
                OrderProductInputModel.Description = Products.Description;
                OrderProductInputModel.Quantity = Products.Quantity;
                OrderProductsOverView.Add(OrderProductInputModel);
            }
            return OrderProductsOverView;

        }
       

    }
}
