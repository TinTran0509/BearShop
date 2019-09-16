using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Web.Core;
using Web.Model;

namespace Web.Repository.Entity
{
    public class VideoRepository : IVideoRepository
    {
        readonly BearShopEntities _entities = new BearShopEntities();
        private const string KeyCache = "cachevideo"; 
        public void Add(Video model)
        {
            HelperCache.RemoveCache(KeyCache);
            object[] parameters =
            {
                new SqlParameter("@Name", model.Name),
                new SqlParameter("@Url",model.Url),
                new SqlParameter("@Images",(object)model.Images??DBNull.Value),
                new SqlParameter("@Status", model.Status),
                new SqlParameter("@CreatedBy", model.CreatedBy),
                new SqlParameter("@Type", model.Type),
                new SqlParameter("@Ordering", model.Ordering),
                new SqlParameter("@IsShowPlay", model.IsShowPlay)
            };
            _entities.Database.ExecuteSqlCommand("Sp_Video_Insert @Name,@Url,@Images,@Status,@CreatedBy,@Type,@Ordering,@IsShowPlay", parameters);
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.Videos.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Video model)
        {
            HelperCache.RemoveCache(KeyCache);
            object[] parameters =
            {
                new SqlParameter("@ID", model.ID),
                new SqlParameter("@Name",model.Name),
                new SqlParameter("@Status", model.Status),
                new SqlParameter("@Ordering", model.Ordering),
                new SqlParameter("@IsShowPlay", model.IsShowPlay)
            };
            _entities.Database.ExecuteSqlCommand("Sp_Video_Update @ID,@Name,@Status,@Ordering,@IsShowPlay", parameters);
        }

        public Video Find(int id)
        {
            return _entities.Videos.Find(id);
        }

        public IEnumerable<Video> GetAll()
        {
            return _entities.Database.SqlQuery<Video>("Sp_Video_GetAll");
        }
        public IEnumerable<VideoModel> GetByName(string name)
        {
            return _entities.Database.SqlQuery<VideoModel>("Sp_Video_GetByName @keyword", new SqlParameter("@keyword", name)).ToList();
        }
    }
}
