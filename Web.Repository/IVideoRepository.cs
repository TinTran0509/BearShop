using System.Collections.Generic;
using Web.Model;

namespace Web.Repository
{
    public interface IVideoRepository
    {
        IEnumerable<VideoModel> GetByName(string name);
        IEnumerable<Video> GetAll();
        Video Find(int id);
        void Add(Video obj);
        void Edit(Video obj);
        void Delete(int id);
    }
}
