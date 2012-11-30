using FubuMVC.Core.Urls;
using FubuMVC.TestingHarness;
using FubuValidation.StructureMap;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace FubuMVC.Validation.IntegrationTesting
{
    public class ValidationHarness : FubuRegistryHarness
    {
        private IContainer theContainer;

        protected override void configureContainer(IContainer container)
        {
            theContainer = container;
            container.Configure(x => x.AddRegistry<ValidationTestRegistry>());
        }

        public IUrlRegistry Urls { get { return theContainer.GetInstance<IUrlRegistry>(); } }

        public class ValidationTestRegistry : Registry
        {
            public ValidationTestRegistry()
            {
                this.FubuValidation();
            }
        }
    }
}