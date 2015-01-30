using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.OrderModels
{
    [Serializable]
    public class OrderRequestInputModel
    {
        public List<int> Id { get; set; }
        public List<int> Quantity { get; set; }

        public List<string> ProductName { get; set; }
        public List<string> Description { get; set; }

    }
}
