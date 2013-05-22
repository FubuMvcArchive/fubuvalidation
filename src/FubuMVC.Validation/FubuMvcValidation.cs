using FubuMVC.Core;
using FubuMVC.Core.UI;
using FubuMVC.Validation.Diagnostics;
using FubuMVC.Validation.Remote;
using FubuMVC.Validation.UI;

namespace FubuMVC.Validation
{
    public class FubuMvcValidation : IFubuRegistryExtension
    {
        void IFubuRegistryExtension.Configure(FubuRegistry registry)
        {
            registry.Services<FubuValidationServiceRegistry>();
            registry.Services<FubuMvcValidationServices>();
            registry.Actions.FindWith<RemoteRulesSource>();
            registry.Actions.FindWith<ValidationSummarySource>();
	        registry.Actions.FindWith<ValidationDiagnosticsSource>();

            registry.Import<HtmlConventionRegistry>(x =>
            {
                x.Editors.Add(new FieldValidationElementModifier());
                x.Editors.Add(new RemoteValidationElementModifier());
                x.Editors.Add(new DateElementModifier());
                x.Editors.Add(new NumberElementModifier());
                x.Editors.Add(new MaximumLengthModifier());
                x.Editors.Add(new MinimumLengthModifier());
                x.Editors.Add(new RangeLengthModifier());
                x.Editors.Add(new MinValueModifier());
                x.Editors.Add(new MaxValueModifier());
                x.Editors.Add(new LocalizationLabelModifier());
                
                x.Forms.Add(new FormValidationSummaryModifier());
                x.Forms.Add(new FormValidationModifier());
				x.Forms.Add(new NotificationSerializationModifier());
            });

            registry.Policies.Add<ValidationConvention>();
            registry.Policies.Add<AttachDefaultValidationSummary>();
            registry.Policies.Add<RegisterRemoteRuleQuery>();

            registry.AlterSettings<ValidationSettings>(settings =>
            {
                settings
                    .Remotes
                    .FindWith<RemoteRuleAttributeFilter>()
                    .FindWith<RemoteFieldValidationRuleFilter>();
            });
        }
    }
}