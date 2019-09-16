using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IAdvImageRepository
    {
        List<AdvImag> GetAll();
        AdvImag Find(int id);
        void Add(AdvImag obj);
        void Edit(AdvImag obj);
        void Delete(int id);
    }
}
