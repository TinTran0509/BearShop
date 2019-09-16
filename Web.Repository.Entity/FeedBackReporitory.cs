using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class FeedBackReporitory : IFeedBackReporitory
    {
        readonly BearShopEntities _entities = new BearShopEntities();
        public void Add(tbl_FeedBack model)
        {
            object[] parameters =
           {
                new SqlParameter("@FullName", model.FullName),
                new SqlParameter("@Phone", model.Phone),
                new SqlParameter("@Email",(object)model.Email ??DBNull.Value),
                new SqlParameter("@Title",model.Title),
                new SqlParameter("@Contents", model.Contents),
                new SqlParameter("@Status",false),
              
            };
            _entities.Database.ExecuteSqlCommand("Sp_Feedback_Insert @FullName,@Phone,@Email,@Title,@Contents,@Status", parameters);
        }
        public void Reply(tbl_FeedBack model)
        {
             object[] parameters =
            {
                new SqlParameter("@ID", model.ID),
                new SqlParameter("@Status",model.Status),

            };
            _entities.Database.ExecuteSqlCommand("Sp_Feedback_Reply @ID,@Status", parameters);
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_FeedBack.Remove(obj);
            _entities.SaveChanges();
        }
        public tbl_FeedBack Find(int id)
        {
            return _entities.tbl_FeedBack.Find(id);
        }
        public IEnumerable<tbl_FeedBack> GetAll()
        {
            return _entities.tbl_FeedBack.OrderByDescending(g => g.CreatedDate);
        }
    }
}
