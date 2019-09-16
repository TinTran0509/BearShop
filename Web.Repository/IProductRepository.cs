using System.Collections.Generic;
using Web.Model;
using Web.Model.CustomModel;

namespace CMS.IRepository
{
    public interface IProductRepository
    {
        IEnumerable<ProductModel> ListAll(int categoryId,string keyWork);
        IEnumerable<Product> GetAll();
        void UpdateImages(int id, string images);
        void Add(Product model);
        void Delete(int productid);
        Product Find(int id);
        void Edit(Product collection);
        List<ProductModel> ProductGetByCategory(string linkseo);
        int TotalProduct();
        Product CheckProductCode(string code);
    }
}

