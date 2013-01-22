namespace FubuValidation.Docs
{
    public class FubuValidationMainTopicTopicRegistry : FubuDocs.TopicRegistry
    {
        public FubuValidationMainTopicTopicRegistry()
        {
            For<FubuValidation.Docs.FubuValidationMainTopic>().Append<FubuValidation.Docs.GettingStarted.InstallingFubumvcvalidation>();
            For<FubuValidation.Docs.FubuValidationMainTopic>().Append<FubuValidation.Docs.GettingStarted.AddingRulesToYourModel>();
            For<FubuValidation.Docs.FubuValidationMainTopic>().Append<FubuValidation.Docs.GettingStarted.BuiltInRules>();
            For<FubuValidation.Docs.FubuValidationMainTopic>().Append<FubuValidation.Docs.GettingStarted.BuildingYourFirstValidatedForm>();
            For<FubuValidation.Docs.FubuValidationMainTopic>().Append<FubuValidation.Docs.Configuration.OutOfTheBoxConventions>();
            For<FubuValidation.Docs.FubuValidationMainTopic>().Append<FubuValidation.Docs.Configuration.PerrouteConfiguration>();
            For<FubuValidation.Docs.FubuValidationMainTopic>().Append<FubuValidation.Docs.Configuration.ConfiguringValidationRules>();
            For<FubuValidation.Docs.FubuValidationMainTopic>().Append<FubuValidation.Docs.Customization.CreatingYourOwnRules>();
            For<FubuValidation.Docs.FubuValidationMainTopic>().Append<FubuValidation.Docs.Customization.ConfiguringRemoteRules>();
            For<FubuValidation.Docs.FubuValidationMainTopic>().Append<FubuValidation.Docs.Customization.ConfiguringWhichRoutesHaveValidation>();
            For<FubuValidation.Docs.FubuValidationMainTopic>().Append<FubuValidation.Docs.Customization.CustomizingTheCssAnnotations>();
            For<FubuValidation.Docs.FubuValidationMainTopic>().Append<FubuValidation.Docs.Customization.ReplacingTheValidationSummary>();

            For<FubuValidation.Docs.Configuration.OutOfTheBoxConventions>().Append<FubuValidation.Docs.Configuration.LofiChains>();
            For<FubuValidation.Docs.Configuration.OutOfTheBoxConventions>().Append<FubuValidation.Docs.Configuration.AjaxChains>();
            For<FubuValidation.Docs.Configuration.OutOfTheBoxConventions>().Append<FubuValidation.Docs.Configuration.RenderingStrategies>();
            For<FubuValidation.Docs.Configuration.OutOfTheBoxConventions>().Append<FubuValidation.Docs.Configuration.RemoteRules>();

            For<FubuValidation.Docs.Configuration.PerrouteConfiguration>().Append<FubuValidation.Docs.Configuration.ChangingRenderingStrategies>();

            For<FubuValidation.Docs.Configuration.ConfiguringValidationRules>().Append<FubuValidation.Docs.Configuration.UsingValidationAttributes>();
            For<FubuValidation.Docs.Configuration.ConfiguringValidationRules>().Append<FubuValidation.Docs.Configuration.ValidationDsl>();

        }
    }
}
