using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;

namespace Web.Repository.Entity
{
    public class OrderRepository : IOrderRepository
    {
        readonly BearShopEntities _entities = new BearShopEntities();
        public IEnumerable<tbl_Order> GetAll()
        {
           return   _entities.tbl_Order;
        }
        public List<OrderModel> OrderDetailGetList(int orderId)
        {
            return _entities.Database.SqlQuery<OrderModel>("Sp_OrderDetail_GetList @OrderID", new SqlParameter("@OrderID", orderId)).ToList();
        }
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_Order.Remove(obj);
            _entities.SaveChanges();
        }
        public void Edit(tbl_Order model)
        {
            object[] parameters =
            {
                new SqlParameter("@ID", model.ID),
                new SqlParameter("@Status", model.Status)
            };
            _entities.Database.ExecuteSqlCommand("Sp_Order_Update @ID,@Status", parameters);
        }
        public tbl_Order Find(int id)
        {
            return _entities.tbl_Order.Find(id);
        }

        public List<tbl_Order> GetList(int status,string name,string tungay,string denngay)
        {
            object[] parameters =
            {
                new SqlParameter("@Status",status),
                new SqlParameter("@CustomerName", name),
                new SqlParameter("@TuNgay", tungay),
                new SqlParameter("@DenNgay", denngay)
            };
           return _entities.Database.SqlQuery<tbl_Order>("SP_Order_GetAll @Status,@CustomerName,@TuNgay,@DenNgay", parameters).ToList();
        }
    }
}
