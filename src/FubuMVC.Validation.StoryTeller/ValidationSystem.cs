using FubuMVC.Validation.Serenity;
using Serenity;
using Serenity.Fixtures.Handlers;

namespace FubuMVC.Validation.StoryTeller
{
    public class ValidationSystem : FubuMvcSystem<ValidationApplication>
    {
        public ValidationSystem()
        {
            ElementHandlers.Handlers.Add(new ValidationElementHandler());
        }
    }
}