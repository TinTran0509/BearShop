using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class ContactReporitory : IContactReporitory
    {
        readonly BearShopEntities context = new BearShopEntities();
        public void Add(string content)
        {
            context.Database.ExecuteSqlCommand("Sp_Contact_Insert @Contents", new SqlParameter("@Contents", content));
        }
        public void Edit(Contact model)
        {
            object[] parameters =
            {
                new SqlParameter("@ID", model.ID),
                new SqlParameter("@Contents", model.Contents)
            };
            context.Database.ExecuteSqlCommand("Sp_Contact_Update @ID,@Contents", parameters);
        }

        public Contact Find(int id)
        {
            return context.Contacts.Find(id);
        }

        public IEnumerable<Contact> GetAll()
        {
            return context.Contacts;
        }
    }
}
