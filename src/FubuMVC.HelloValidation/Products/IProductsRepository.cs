using System.Collections.Generic;

namespace FubuMVC.HelloValidation.Products
{
    public interface IProductsRepository
    {
        IEnumerable<Product> GetAll();
        void Insert(Product product);
    }
}