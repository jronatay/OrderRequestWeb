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
        private EntityLibrary.OrderRepository OrderRepository = new OrderRepository(new OrderRequestEntities());
        private List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderInput;
        private EntityLibrary.Order Order;
        private EntityLibrary.OrderItem OrderItem;
        public IEnumerable<OrderProduct> OrderProductList()
        {
            return OrderRepository.OrderProductList();
        }

        public List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductInputList()
        {
            return OrderRepository.OrderProductInputList();
        }

        public List<EntityLibrary.OrderModels.OrderProductsInputModel> PopulatedOrderProductFromRequest(EntityLibrary.OrderModels.OrderRequestInputModel OrderRequestModel)
        {
            return PopulateModelFromRequest(OrderRequestModel);
        }

        public List<EntityLibrary.OrderModels.OrderProductsInputModel> PopulateModelFromRequest(EntityLibrary.OrderModels.OrderRequestInputModel OrderInputRequest)
        {
            List<EntityLibrary.OrderModels.OrderProductsInputModel> OrderProductsModel = new List<EntityLibrary.OrderModels.OrderProductsInputModel>();
            for (int counter = 0; counter < OrderInputRequest.Id.Count(); counter++)
            {
                EntityLibrary.OrderModels.OrderProductsInputModel RequestedProduct = new EntityLibrary.OrderModels.OrderProductsInputModel();
                RequestedProduct.Id = OrderInputRequest.Id[counter];
                RequestedProduct.ProductName = OrderInputRequest.ProductName[counter];
                RequestedProduct.Description = OrderInputRequest.Description[counter];
                RequestedProduct.Quantity = OrderInputRequest.Quantity[counter];
                OrderProductsModel.Add(RequestedProduct);

            }
            return OrderProductsModel;

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
            EntityLibrary.OrderModels.OrderProductsInputModel OrderProductsInput = new EntityLibrary.OrderModels.OrderProductsInputModel();
            OrderProductsInput.Id = OrderProductsRequestedInput.Id;
            OrderProductsInput.ProductName = OrderProductsRequestedInput.ProductName;
            OrderProductsInput.Description = OrderProductsRequestedInput.Description;
            OrderProductsInput.Quantity = OrderProductsRequestedInput.Quantity;
            return OrderProductsInput;
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
            Order = new Order();
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
           
            OrderInput = new List<EntityLibrary.OrderModels.OrderProductsInputModel>();
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
            OrderItem = new OrderItem();
            OrderItem.Quantity = OrderProducts.Quantity;
            OrderItem.OrderProductID = OrderProducts.Id;
            OrderItem.OrderId = OrderNo;
            return OrderItem;
        }
        public OrderItem AddOrderItemsOtherProducts(EntityLibrary.OrderModels.OrderProductsInputModel OrderProducts, int OrderNo,int OrderProductID)
        {
            OrderItem = new OrderItem();
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



    }
}
