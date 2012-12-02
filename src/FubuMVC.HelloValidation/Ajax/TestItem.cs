using FubuValidation;

namespace FubuMVC.HelloValidation.Ajax
{
    public class TestItem
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}