using FubuMVC.Core.Continuations;

namespace FubuMVC.HelloValidation.Handlers.Products.Create
{
    public class PostHandler
    {
        private readonly IProductsRepository _repository;

        public PostHandler(IProductsRepository repository)
        {
            _repository = repository;
        }

        public FubuContinuation Execute(CreateProductInputModel inputModel)
        {
            _repository.Insert(new Product
                                   {
                                       Name = inputModel.Name,
                                       Price = inputModel.Price
                                   });
            return FubuContinuation.RedirectTo(new ProductsListRequestModel());
        }
    }
}