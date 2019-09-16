using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;
using Web.Model.CustomModel;

namespace Web.Repository
{
    public interface IOrderRepository
    {
        List<tbl_Order> GetList(int status, string name, string tungay, string denngay);
        IEnumerable<tbl_Order> GetAll();
        void Delete(int id);
        tbl_Order Find(int id);
        void Edit(tbl_Order model);
        List<OrderModel> OrderDetailGetList(int orderId);
    }
}
