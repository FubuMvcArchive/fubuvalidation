using System.Collections.Generic;

namespace FubuMVC.HelloValidation.Products
{
    public class ProductsRepository : IProductsRepository
    {
        private static readonly List<Product> Products = new List<Product>();

        public IEnumerable<Product> GetAll()
        {
            return Products;
        }

        public void Insert(Product product)
        {
            Products.Fill(product);
        }
    }
}