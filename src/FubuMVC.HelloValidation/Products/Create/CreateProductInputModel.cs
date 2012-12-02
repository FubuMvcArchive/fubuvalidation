using FubuValidation;

namespace FubuMVC.HelloValidation.Products.Create
{
    public class CreateProductInputModel
    {
        [Required]
        public string Name { get; set; }
        public double Price { get; set; }

        [ContinueValidation]
        public Hanging Hanging { get; set; }
    }

    public class Hanging
    {
        [Required]
        public string Fruit { get; set; }
    }
}