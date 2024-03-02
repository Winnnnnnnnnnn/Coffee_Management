using System;
using System.Collections.Generic;
using System.Linq;
using MyLibrary.DataAccess;

namespace MyLibrary.Repository
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<Product> GetProducts() => ProductDAO.Instance.GetProductList();
        public Product GetProductByID(int product_id) => ProductDAO.Instance.GetProductByID(product_id);
        public void InsertProduct(Product product)
        {
            ProductDAO.Instance.AddNew(product);
        }
        public void DeleteProduct(int product_id) => ProductDAO.Instance.Remove(product_id);
        public void DeleteProducts(List<int> product_ids) => ProductDAO.Instance.RemoveMultiple(product_ids);
        public void UpdateProduct(Product product) => ProductDAO.Instance.Update(product);
    }
}
