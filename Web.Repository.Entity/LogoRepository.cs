using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core;
using Web.Model;

namespace Web.Repository.Entity
{
    public class LogoRepository : ILogoRepository
    {
        readonly BearShopEntities _entities = new BearShopEntities();
        private const string KeyCache = "cacheadminmenu"; 
        public void Add(Logo obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Logoes.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.Logoes.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Logo obj)
        {
            HelperCache.RemoveCache(KeyCache);
            object[] parameters =
         {
                new SqlParameter("@ID", obj.ID),
                  new SqlParameter("@Name", obj.Name),
                 new SqlParameter("@Image", obj.Image),
                  new SqlParameter("@Type", obj.Type),
                  new SqlParameter("@Status", obj.Status)
            };
            _entities.Database.ExecuteSqlCommand("Sp_Logo_Update @ID,@Name,@Image,@Type,@Status", parameters);
        }

        public Logo Find(int id)
        {
            return _entities.Logoes.Find(id);
        }
        public void Active(int id,bool status)
        {
            object[] parameters =
          {
                new SqlParameter("@ID", id),
                new SqlParameter("@Status", status)
            };
            _entities.Database.ExecuteSqlCommand("Sp_Logo_Active @ID,@Status", parameters);
        }
        public List<Logo> GetAll()
        {
            var lstData = HelperCache.GetCache<List<Logo>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.Logoes.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
