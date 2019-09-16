using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IContactReporitory
    {
        IEnumerable<Contact> GetAll();
        Contact Find(int id);
        void Add(string content);
        void Edit(Contact model);
    }
}
