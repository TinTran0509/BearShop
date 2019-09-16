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
    public class CartRepository : ICartRepository
    {
        readonly BearShopEntities _entities = new BearShopEntities();
        private const string KeyCache = "cachecategories"; 
        private const string KeyCacheView = "cachecategories_view";
        public IEnumerable<tbl_Order> GetAll()
        {
            return _entities.tbl_Order;
        }
        public void AddOrderDetail(OrderDetail model)
        {
            HelperCache.RemoveCache(KeyCache);
            object[] parameters =
            {
                new SqlParameter("@ProductID",model.ProductID),
                new SqlParameter("@OrderID", model.OrderID),
                new SqlParameter("@Quantity", model.Quantity) 
            };
            _entities.Database.ExecuteSqlCommand("Sp_OrderDetail_Insert @ProductID,@OrderID,@Quantity", parameters);
        }
        public int AddOrder(tbl_Order model)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_Order.Add(model);
            _entities.SaveChanges();
            return model.ID;
            //object[] parameters =
            //{
            //    new SqlParameter("@CustomerName", model.CustomerName),
            //    new SqlParameter("@CustomerAddress",model.CustomerAddress),
            //    new SqlParameter("@CustomerPhone", model.CustomerPhone),
            //    new SqlParameter("@CustomerEmail", model.CustomerEmail),
            //    new SqlParameter("@Status",false)
            //};
            //_entities.Database.ExecuteSqlCommand("Sp_Order_Insert @CustomerName,@CustomerAddress,@CustomerPhone,@CustomerEmail,@Status", parameters);
        }
    }
}
