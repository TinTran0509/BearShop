using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core;
using Web.Model;

namespace Web.Repository.Entity
{
    public class CountAccessRepository : ICountAccessRepository
    {
        readonly BearShopEntities _entities = new BearShopEntities();

        public void Edit(Model.tbl_CountAccess obj)
        {            
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_CountAccess Find(int id)
        {
            return _entities.tbl_CountAccess.Find(id);
        }

    }
}
