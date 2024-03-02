using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DataAccess;

namespace MyLibrary.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProductByID(int product_id);
        void InsertProduct(Product product);
        void DeleteProduct(int product_id);
        void DeleteProducts(List<int> product_ids);
        void UpdateProduct(Product product);
    }
}
