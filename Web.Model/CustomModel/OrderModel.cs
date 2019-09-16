using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Model.CustomModel
{
    public partial class OrderModel : Product
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        
    }
}
