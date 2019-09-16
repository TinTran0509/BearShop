using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;

namespace Web.Repository.Entity
{
    public class NewsRepository : INewsRepository
    {
        readonly BearShopEntities context = new BearShopEntities();
       
        public void Add(News model)
        {
            object[] parameters =
            {
                new SqlParameter("@CategoryId", model.CategoryId),
                new SqlParameter("@MetaTitle", model.MetaTitle),
                new SqlParameter("@Image",(object)model.Image??DBNull.Value),
                new SqlParameter("@Desciption", model.Desciption),
                new SqlParameter("@CreatedBy", model.CreatedBy),
                new SqlParameter("@Contents", model.Contents),
                new SqlParameter("@Status", model.Status),
                new SqlParameter("@Tags",(object)model.Tags??DBNull.Value)
            };
            context.Database.ExecuteSqlCommand("Sp_News_Insert @CategoryId,@MetaTitle,@Image,@Desciption,@CreatedBy,@Contents,@Status,@Tags", parameters);
        }

        public IEnumerable<ListNews> ListAll(string keyWord, int status)
        {
            object[] parameters =
            {
                new SqlParameter("@MetaTitle",keyWord),
                new SqlParameter("@Status",status),
            };
            return context.Database.SqlQuery<ListNews>("Sp_News_ListAll @MetaTitle,@Status", parameters);
        }
        public IEnumerable<News> GetAll()
        {
            return context.News;
        }
        public void Delete(int id)
        {
            context.Database.ExecuteSqlCommand("Sp_News_Delete @ID ", new SqlParameter("@ID", id));
        }
        public void Publish(int id)
        {
            context.Database.ExecuteSqlCommand("Sp_News_Publish @ID ", new SqlParameter("@ID", id));
        }
        public void UnPublish(int id)
        {
            context.Database.ExecuteSqlCommand("Sp_News_UnPublish @ID ", new SqlParameter("@ID", id));
        }
        public void Edit(News model)
        {
            object[] parameters =
            {
                new SqlParameter("@ID", model.ID),
                new SqlParameter("@CategoryId", model.CategoryId),
                new SqlParameter("@MetaTitle", model.MetaTitle),
                new SqlParameter("@Image", (object)model.Image??DBNull.Value),
                new SqlParameter("@Desciption", model.Desciption),
                new SqlParameter("@ModifiedBy", 2),
                new SqlParameter("@Contents", model.Contents),
                new SqlParameter("@Status", model.Status),
                new SqlParameter("@Tags",(object)model.Tags??DBNull.Value)
            };
            context.Database.ExecuteSqlCommand(" Sp_News_Update @ID,@CategoryId,@MetaTitle,@Image,@Desciption,@ModifiedBy,@Contents,@Status,@Tags", parameters);
        }
        public News FindByTitle(string title)
        {
            return context.Database.SqlQuery<News>("Sp_News_GetByTitle @MetaTitle", new SqlParameter("@MetaTitle", title)).FirstOrDefault();
        }
        public News Find(int id)
        {
            return context.Database.SqlQuery<News>("Sp_News_Find @ID", new SqlParameter("@ID", id)).FirstOrDefault();
        }
        public ListNews Detail(int id)
        {
            return context.Database.SqlQuery<ListNews>("Sp_News_Detail @ID", new SqlParameter("@ID", id)).FirstOrDefault();
        }
        public List<ListNews> NewsGetByCategory(string linkseo)
        {
            return context.Database.SqlQuery<ListNews>("Sp_News_GetByCategory @LinkSeo", new SqlParameter("@LinkSeo", linkseo)).ToList();
        }
    }
}
