using FubuMVC.Core.Continuations;

namespace FubuMVC.HelloValidation.Products.Create
{
    public class CreateProductEndpoint
    {
        private readonly IProductsRepository _repository;

        public CreateProductEndpoint(IProductsRepository repository)
        {
            _repository = repository;
        }

        public CreateProductInputModel get_create_product(CreateProductInputModel request)
        {
            return request;
        }

        public FubuContinuation post_create_product(CreateProductInputModel input)
        {
            _repository.Insert(new Product
            {
                Name = input.Name,
                Price = input.Price
            });

            return FubuContinuation.RedirectTo(new ProductsListRequestModel());
        }
    }
}