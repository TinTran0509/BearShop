using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IAboutRepository
    {
        List<About> GetAll();
        void Add(About model);
        void Delete(int id);
        About Find(int id);
        void Edit(About model);
    }
}
