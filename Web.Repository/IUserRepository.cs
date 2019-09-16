using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User Find(int id);
        void Add(User obj);
        void Edit(User obj);
        void Edit(List<User> lstobj);
        void Delete(int id);
    }
}
