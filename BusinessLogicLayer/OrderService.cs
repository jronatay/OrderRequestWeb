using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EntityLibrary;

namespace BusinessLogicLayer
{
    public class OrderService
    {
        private EntityLibrary.OrderRepository OrderRepository = new OrderRepository();
        private List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderInput = new List<EntityLibrary.OrderModels.OrderProductsInputModel>();
        private EntityLibrary.Order Order = new EntityLibrary.Order();
        private EntityLibrary.OrderItem OrderItem = new OrderItem();
        private EntityLibrary.OrderModels.OrderProductsInputModel OrderProductInputModel = new EntityLibrary.OrderModels.OrderProductsInputModel();
        public IEnumerable<OrderProduct> OrderProductList()
        {
            return OrderRepository.OrderProductList();
        }

        public List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductInputList()
        {
            return OrderProductInputLists();
        }
        public List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductInputLists()
        {
            return PopulateOrderProductInputModel(OrderRepository.OrderProductList());
        }
        public List<EntityLibrary.OrderModels.OrderProductsInputModel> PopulateOrderProductInputModel(List<OrderProduct> OrderProducts)
        {
            List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductInputLists = new List<EntityLibrary.OrderModels.OrderProductsInputModel>();

            OrderProducts = OrderRepository.OrderProductList();
            foreach (var OrderProductsItem in OrderProducts)
            {
                EntityLibrary.OrderModels.OrderProductsInputModel OrderProductInput = new EntityLibrary.OrderModels.OrderProductsInputModel();
                OrderProductInput.Id = OrderProductsItem.Id;
                OrderProductInput.ProductName = OrderProductsItem.ProductName;
                OrderProductInput.Description = OrderProductsItem.Description;
                OrderProductInput.Quantity = 0;
                OrderProductInputLists.Add(OrderProductInput);
            }
            return OrderProductInputLists;
        }

        public List<EntityLibrary.OrderModels.OrderProductsInputModel> PopulatedOrderProductFromRequest(EntityLibrary.OrderModels.OrderRequestInputModel OrderRequestModel)
        {
            return PopulateModelFromRequest(OrderRequestModel);
        }

    
        public List<EntityLibrary.OrderModels.OrderProductsInputModel> PopulateModelFromRequest(EntityLibrary.OrderModels.OrderRequestInputModel OrderInputRequest)
        {
           
            for (int counter = 0; counter < OrderInputRequest.Id.Count(); counter++)
            {
                EntityLibrary.OrderModels.OrderProductsInputModel OrderProductInputModel = new EntityLibrary.OrderModels.OrderProductsInputModel();
                OrderProductInputModel.Id = OrderInputRequest.Id[counter];
                OrderProductInputModel.ProductName = OrderInputRequest.ProductName[counter];
                OrderProductInputModel.Description = OrderInputRequest.Description[counter];
                OrderProductInputModel.Quantity = OrderInputRequest.Quantity[counter];
                OrderInput.Add(OrderProductInputModel);

            }
            return OrderInput;

        }

        public Boolean ValidateOrderRequestedInputs(List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsInput)
        {
            return true;
        }

        public Boolean IsThereAtleastOneOrder(List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsInput)
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

        public List<EntityLibrary.OrderModels.OrderProductsInputModel> ReturnStoredOrderProducts(List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsInput)
        {
            if (IsThereAtleastOneOrder(OrderProductsInput))
            {
                return StoreProductsOnTemporaryStorage(OrderProductsInput);
            }
            return null;
        }

        public List<EntityLibrary.OrderModels.OrderProductsInputModel> StoreProductsOnTemporaryStorage(List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsRequestedInput)
        {
            List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsTemporaryStorage = new List<EntityLibrary.OrderModels.OrderProductsInputModel>();
            foreach (var Item in OrderProductsRequestedInput)
            {
                if (Item.Quantity > 0)
                {
                    OrderProductsTemporaryStorage.Add(PopulateTemporaryStorage(Item));
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

        public Boolean IsRequestedStoredInTemporaryStorage(List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsInput)
        {
            return ReturnStoredOrderProducts(OrderProductsInput) != null;
        }

       
        public List<EntityLibrary.OrderModels.OrderProductsInputModel> ReturnOrderProductsStored(List<EntityLibrary.OrderModels.OrderProductsInputModel> StoredOrderProducts)
        {
            if (IsRequestedStoredInTemporaryStorage(StoredOrderProducts))
            {
                return ReturnStoredOrderProducts(StoredOrderProducts);
            }
            return null;
        }

        public Boolean IsConfirmedOrderRequestNull(EntityLibrary.OrderModels.OrderRequestInputModel ConfirmedOrderRequest)
        {
            return ConfirmedOrderRequest == null;
        }

       
        public Boolean IsOrderCreatedInDatabase()
        {
            return true;
        }
        public Order PopulateOrder(int CustomerID)
        {
           
            Order.CreationDate = DateTime.Now.ToUniversalTime();
            Order.CustomerId = CustomerID;
            return Order;
        }
        public int NewOrder(int CustomerID)
        {
           
            int OrderNo = OrderRepository.AddOrderRequestAndReturnGeneratedID(CustomerID);
            OrderRepository.UpdateOrderNo(OrderNo);
            return OrderNo;
        }

        #region OrderItems
        public void SaveOrder(EntityLibrary.OrderModels.OrderRequestInputModel ConfirmedOrderRequest,int OrderNo)
        {
           
           
            OrderInput = PopulatedOrderProductFromRequest(ConfirmedOrderRequest);
            foreach (var Product in OrderInput)
            {
                if (IsConfirmedOrderProductOthers(Product.Id)  && !IsConfirmedOrderProductOthersZeroQuantity(Product.Quantity))
                {
                    int OrderProductID =OrderRepository.AddOtherProductAndReturnGeneratedID(Product);
                    OrderRepository.AddOrderItems(AddOrderItemsOtherProducts(Product, OrderNo, OrderProductID));
                }
                else if (!IsConfirmedOrderProductOthers(Product.Id) && !IsConfirmedOrderProductOthersZeroQuantity(Product.Quantity))
                {
                    OrderRepository.AddOrderItems(AddOrderItems(Product, OrderNo));
                }
            }
        }
        public Boolean IsConfirmedOrderProductOthers(int Id)
        {
            return Id == 0;
        }
        public Boolean IsConfirmedOrderProductOthersZeroQuantity(int Quantity)
        {
            return Quantity == 0;
        }
        public OrderItem AddOrderItems(EntityLibrary.OrderModels.OrderProductsInputModel OrderProducts,int OrderNo)
        {
           
            OrderItem.Quantity = OrderProducts.Quantity;
            OrderItem.OrderProductID = OrderProducts.Id;
            OrderItem.OrderId = OrderNo;
            return OrderItem;
        }
        public OrderItem AddOrderItemsOtherProducts(EntityLibrary.OrderModels.OrderProductsInputModel OrderProducts, int OrderNo,int OrderProductID)
        {
           
            OrderItem.Quantity = OrderProducts.Quantity;
            OrderItem.OrderProductID = OrderProductID;
            OrderItem.OrderId = OrderNo;
            return OrderItem;
        }
        public Order OrderConfirmation(int OrderNo)
        {
            return OrderRepository.OrderConfirmation(OrderNo);
        }
        #endregion

        public List<EntityLibrary.Order> GetOrderByCustomerID(int CustomerId)
        {
            return OrderRepository.GetOrderByCustomer(CustomerId);
        }
        public List<EntityLibrary.OrderModels.OrderProductsInputModel> GetOrderProductsByOrderIdAndCustomerId(int OrderId,int CustomerId)
        {
            return OrderRepository.GetOrderDetails(CustomerId, OrderId);
        }


    }
}
