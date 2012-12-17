using StructureMap.Configuration.DSL;

namespace FubuValidation.StructureMap
{
    public static class ValidationBootstrapExtensions
    {
        public static void FubuValidation(this Registry registry)
        {
            registry.IncludeRegistry<FubuValidationRegistry>();
        }
    }
}