using FubuMVC.Core;
using FubuMVC.HelloValidation.Products;

namespace FubuMVC.HelloValidation
{
    public class HelloValidationFubuRegistry : FubuRegistry
    {
        public HelloValidationFubuRegistry()
        {
            Routes.HomeIs<ProductListingEndpoint>(h => h.get_products_list(new ProductsListRequestModel()));
        }
    }
}