using FubuValidation;

namespace FubuMVC.Validation.StoryTeller.Endpoints.ValidationElementHandler
{
    public class ElementHandler
    {
        [Required, Throttled(0)]
        public string AjaxValueFast { get; set; }
        [Required, Throttled(3)]
        public string AjaxValueSlow { get; set; }
        [Required, Throttled(10)]
        public string AjaxValueReallySlow { get; set; }
        [Required]
        public string SynchronousValue { get; set; }
    }
}