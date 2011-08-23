using System.Collections.Generic;
using System.Linq;

namespace FubuMVC.HelloValidation.Handlers.Products
{
    public class GetHandler
    {
        private readonly IProductsRepository _repository;

        public GetHandler(IProductsRepository repository)
        {
            _repository = repository;
        }

        public ProductsListViewModel Execute(ProductsListRequestModel request)
        {
            return new ProductsListViewModel
                       {
                           Products = _repository.GetAll()
                       };
        }
    }

    public class ProductsListViewModel
    {
        public ProductsListViewModel()
        {
            Products = new List<Product>();
        }

        public IEnumerable<Product> Products { get; set; }

        public bool Any()
        {
            return Products.Any();
        }
    }
}