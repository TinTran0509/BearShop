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
    public class HomeMenuRepository : IHomeMenuRepository
    {
        readonly BearShopEntities _entities = new BearShopEntities();
        private const string KeyCache = "HomeMenu";
        public void Add(HomeMenu model)
        {
            if (!string.IsNullOrEmpty(model.Link))
            {
                model.Link = model.Link.Trim();
            }
            object[] parameters =
             {
                new SqlParameter("@Name",model.Name ),
                new SqlParameter("@Link", (object)model.Link?? DBNull.Value),
                new SqlParameter("@LinkSeo",(object)model.LinkSeo?? DBNull.Value),
                new SqlParameter("@Icon",(object) model.Icon?? DBNull.Value),
                new SqlParameter("@ParentId", model.ParentId),
                new SqlParameter("@Ordering", model.Ordering)
            };
            _entities.Database.ExecuteSqlCommand("Sp_HomeMenu_Insert @Name,@Link,@LinkSeo,@Icon,@ParentId,@Ordering", parameters);
        }
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.HomeMenus.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(HomeMenu model)
        {
            if (!string.IsNullOrEmpty(model.Link))
            {
                model.Link = model.Link.Trim();
            }
            object[] parameters =
            {
                new SqlParameter("@ID", model.ID),
                new SqlParameter("@Name", model.Name),
                new SqlParameter("@LinkSeo",(object)model.LinkSeo?? DBNull.Value),
                new SqlParameter("@Link", (object)model.Link?? DBNull.Value),
                new SqlParameter("@Icon",(object) model.Icon?? DBNull.Value),
                new SqlParameter("@ParentId", model.ParentId),
                new SqlParameter("@Ordering", model.Ordering)
            };
            _entities.Database.ExecuteSqlCommand("Sp_HomeMenu_Update @ID,@Name,@Link,@LinkSeo,@Icon,@ParentId,@Ordering", parameters);
        }

        public HomeMenu Find(int id)
        {
            return _entities.HomeMenus.Find(id);
        }

        public List<HomeMenu> GetAll()
        {
            return _entities.Database.SqlQuery<HomeMenu>("Sp_HomeMenu_GetAll").ToList();
        }
    }
}
