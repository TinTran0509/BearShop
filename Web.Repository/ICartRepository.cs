using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;
using Web.Model.CustomModel;

namespace Web.Repository
{
    public interface ICartRepository
    {
        IEnumerable<tbl_Order> GetAll();
        void AddOrderDetail(OrderDetail model);
        int AddOrder(tbl_Order model);
    }
}
