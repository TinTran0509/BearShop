using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IPolicyRepository
    {
        List<Policy> GetAll();
        void Add(Policy model);
        void Delete(int id);
        Policy Find(int id);
        void Edit(Policy model);
    }
}
