using FubuValidation;

namespace FubuMVC.HelloValidation.Handlers.Products.Create
{
    public class CreateProductInputModel
    {
        [Required]
        public string Name { get; set; }
        public double Price { get; set; }
    }
}