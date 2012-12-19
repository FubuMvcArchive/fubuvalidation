using FubuMVC.Core.Urls;
using FubuMVC.TestingHarness;
using StructureMap;

namespace FubuMVC.Validation.IntegrationTesting
{
    public class ValidationHarness : FubuRegistryHarness
    {
        private IContainer theContainer;

        protected override void configureContainer(IContainer container)
        {
            theContainer = container;
        }

        public IUrlRegistry Urls { get { return theContainer.GetInstance<IUrlRegistry>(); } }

        public T Get<T>()
        {
            return theContainer.GetInstance<T>();
        }
    }
}