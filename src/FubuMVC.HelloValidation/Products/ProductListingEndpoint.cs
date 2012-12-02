using System.Collections.Generic;
using System.Linq;

namespace FubuMVC.HelloValidation.Products
{
    public class ProductListingEndpoint
    {
        private readonly IProductsRepository _repository;

        public ProductListingEndpoint(IProductsRepository repository)
        {
            _repository = repository;
        }

        public ProductsListViewModel get_products_list(ProductsListRequestModel request)
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