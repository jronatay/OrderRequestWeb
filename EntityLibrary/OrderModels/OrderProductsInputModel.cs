using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.OrderModels
{
   public  class OrderProductsInputModel
    {
       public int Id { get; set; }
       public int Quantity { get; set; }
       
       public string ProductName { get; set; }
       public string Description { get; set; }
    }
}
