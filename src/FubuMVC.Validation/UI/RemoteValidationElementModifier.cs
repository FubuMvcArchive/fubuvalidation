using System.Linq;
using FubuMVC.Core.UI.Elements;
using FubuMVC.Core.Urls;
using FubuMVC.Validation.Remote;

namespace FubuMVC.Validation.UI
{
    public class RemoteValidationElementModifier : IElementModifier
    {
        public bool Matches(ElementRequest token)
        {
            return true;
        }

        public void Modify(ElementRequest request)
        {
            var tag = request.CurrentTag;
            if (tag == null || !tag.IsInputElement())
            {
                return;
            }

            var graph = request.Get<RemoteRuleGraph>();
            var rules = graph.RulesFor(request.Accessor);
            var data = new RemoteValidationDef
            {
                rules = rules.Select(x => x.ToHash()).ToArray(),
                url = request.Get<IUrlRegistry>().RemoteRule()
            };

            tag.Data("remote-rule", data);
        }
    }

    public class RemoteValidationDef
    {
        public string url { get; set; }
        public string[] rules { get; set; }
    }
}