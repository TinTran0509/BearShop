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
    public class CategoryRepository : ICategoryRepository
    {
        readonly BearShopEntities _entities = new BearShopEntities();

        public void Add(Category model)
        {
            object[] parameters =
             {
                new SqlParameter("@Name",model.Name ),
                new SqlParameter("@LinkSeo",(object)model.LinkSeo?? DBNull.Value),
                new SqlParameter("@ParentID", model.ParentID),
                new SqlParameter("@Ordering", model.Ordering)
            };
            _entities.Database.ExecuteSqlCommand("Sp_Category_Insert @Name,@LinkSeo,@ParentID,@Ordering", parameters);
        }
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.Categories.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Category model)
        {
            object[] parameters =
            {
                new SqlParameter("@ID", model.ID),
                new SqlParameter("@Name",model.Name ),
                new SqlParameter("@LinkSeo",model.LinkSeo),
                new SqlParameter("@ParentID", model.ParentID),
                new SqlParameter("@Ordering", model.Ordering)
            };
            _entities.Database.ExecuteSqlCommand("Sp_Category_Update @ID,@Name,@LinkSeo,@ParentID,@Ordering", parameters);
        }
        public Category Find(int id)
        {
            return _entities.Categories.Find(id);
        }
        public IEnumerable<Category> GetAll()
        {
            return _entities.Categories;
        }
    }
}
