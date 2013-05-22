using FubuLocalization;
using FubuMVC.Core.UI.Elements;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.UI
{
    [TestFixture]
    public class LocalizationLabelModifierTester : ValidationElementModifierContext<LocalizationLabelModifier>
    {
        [Test]
        public void adds_the_localized_label_data_attribute()
        {
            var theRequest = ElementRequest.For(new TargetModel(), x => x.FirstName);
            var label = LocalizationManager.GetHeader(theRequest.Accessor.InnerProperty);
            tagFor(theRequest).Data(LocalizationLabelModifier.LocalizedLabelKey).ShouldEqual(label);
        }

        public class TargetModel
        {
            public string FirstName { get; set; }
        }
    }
}