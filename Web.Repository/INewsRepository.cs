using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;
using Web.Model.CustomModel;

namespace Web.Repository
{
    public interface INewsRepository
    {
        IEnumerable<ListNews> ListAll(string keyWord, int status);
        IEnumerable<News> GetAll();
        void Add(News model);
        void Delete(int newsid);
        News Find(int id);
        News FindByTitle(string title);
        void Edit(News model);
        void UnPublish(int id);
        void Publish(int id);
        ListNews Detail(int id);
        List<ListNews> NewsGetByCategory(string linkseo);

    }
}
