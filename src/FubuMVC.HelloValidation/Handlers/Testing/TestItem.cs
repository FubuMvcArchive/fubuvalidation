using FubuValidation;

namespace FubuMVC.HelloValidation.Handlers.Testing
{
    public class TestItem
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}