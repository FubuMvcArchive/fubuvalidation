using FubuValidation;

namespace FubuMVC.HelloValidation.Ajax
{
    public class TestItem
    {
        [UniqueUsername]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}