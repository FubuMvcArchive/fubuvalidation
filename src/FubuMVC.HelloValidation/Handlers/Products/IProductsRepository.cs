using System.Collections.Generic;

namespace FubuMVC.HelloValidation.Handlers.Products
{
    public interface IProductsRepository
    {
        IEnumerable<Product> GetAll();
        void Insert(Product product);
    }
}