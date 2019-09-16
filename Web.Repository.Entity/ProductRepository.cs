using CMS.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Web.Model;
using Web.Model.CustomModel;

namespace CMS.Reporitory
{
    public class ProductRepository : IProductRepository
    {
        //LedLoveEntities
        //LedTraiTimEntities
        private readonly BearShopEntities context = new BearShopEntities();
        public void Add(Product model)
        {
            context.Products.Add(model);
            context.SaveChanges();
        }
        public IEnumerable<ProductModel> ListAll(int satatus, string keyWord)
        {
            bool isHot = false;
            if (satatus != 0)
                isHot = true;
            object[] parameters =
            {
               new SqlParameter("@IsHot", isHot),
               new SqlParameter("@KeyWord", keyWord)
            };
            return context.Database.SqlQuery<ProductModel>("Sp_Product_ListAll @IsHot,@KeyWord", parameters);
        }
        public void UpdateImages(int id,string images)
        {
            object[] parameters =
            {
                new SqlParameter("@ID", id),
                new SqlParameter("@ImageMore", images)
            };
            context.Database.ExecuteSqlCommand("Sp_Product_Update_Images @ID,@ImageMore", parameters);
        }
        public IEnumerable<Product> GetAll()
        {
            return context.Products;
        }
        public List<ProductModel> ProductGetByCategory(string linkseo)
        {
            return context.Database.SqlQuery<ProductModel>("Sp_Product_GetByCategory @LinkSeo", new SqlParameter("@LinkSeo", linkseo)).ToList();
        }
        public void Delete(int id)
        {
            context.Database.ExecuteSqlCommand("Sp_Product_Delete @ID", new SqlParameter("@ID", id));
        }
        public void Edit(Product model)
        {
            object[] parameters =
            {
                new SqlParameter("@ID", model.ID),
                new SqlParameter("@CategoryId", (object)model.CategoryId?? DBNull.Value),
                new SqlParameter("@Name", model.Name),
                new SqlParameter("@ProductCode", model.ProductCode),
                new SqlParameter("@Images", model.Images),
                new SqlParameter("@Price", (object)model.Price?? DBNull.Value),
                new SqlParameter("@Sale",  (object)model.Sale?? DBNull.Value),
                new SqlParameter("@Description", model.Description),
                new SqlParameter("@IsHot",(object)model.IsHot?? DBNull.Value),
            };
            context.Database.ExecuteSqlCommand("Sp_Product_Update @ID,@CategoryId,@Name,@ProductCode,@Images,@Price,@Sale,@Description,@IsHot", parameters);
        }
        public Product Find(int id)
        {
            return context.Database.SqlQuery<Product>("Sp_Product_Find @ID", new SqlParameter("@ID", id)).FirstOrDefault();
        }
        public  int TotalProduct()
        {
            return context.Products.Count();
        }
        public Product CheckProductCode(string code)
        {
            return context.Database.SqlQuery<Product>("Sp_Product_CheckCode @ProductCode", new SqlParameter("@ProductCode", code)).FirstOrDefault();
        }
    }
}
