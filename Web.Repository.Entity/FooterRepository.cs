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
    public class FooterRepository : IFooterRepository
    {
        readonly BearShopEntities context = new BearShopEntities();
        public void Add(string content)
        {
            context.Database.ExecuteSqlCommand("Sp_Footer_Insert @Contents", new SqlParameter("@Contents", content));
        }
        public IEnumerable<Footer> GetAll()
        {
            return context.Footers;
        }

        public void Edit(Footer model)
        {
            object[] parameters =
            {
                new SqlParameter("@ID", model.ID),
                new SqlParameter("@Contents", model.Contents)
            };
            context.Database.ExecuteSqlCommand("Sp_Footer_Update @ID,@Contents", parameters);
        }

        public Footer Find(int id)
        {
            return context.Footers.Find(id);
        }
    }
}
