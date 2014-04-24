using FubuMVC.Core.Urls;
using StructureMap;

namespace FubuMVC.Validation.IntegrationTesting
{
    public class ValidationHarness
    {


        private IContainer theContainer;

        protected virtual void configureContainer(IContainer container)
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