using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IFooterRepository
    {
        IEnumerable<Footer> GetAll();
        void Add(string content);
        Footer Find(int id);
        void Edit(Footer model);
    }
}
