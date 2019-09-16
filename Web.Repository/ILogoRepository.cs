using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ILogoRepository
    {
        List<Logo> GetAll();
        Logo Find(int id);
        void Add(Logo obj);
        void Edit(Logo obj);
        void Delete(int id);
        void Active(int id, bool status);
    }
}
