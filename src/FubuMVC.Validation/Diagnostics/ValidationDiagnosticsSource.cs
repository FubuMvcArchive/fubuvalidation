using System.Collections.Generic;
using System.Reflection;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Validation.Diagnostics
{
	public class ValidationDiagnosticsSource : IActionSource
	{
		public IEnumerable<ActionCall> FindActions(Assembly applicationAssembly)
		{
			yield return ActionCall.For<ValidationDiagnostics>(x => x.Ajax(null));
			yield return ActionCall.For<ValidationDiagnostics>(x => x.LoFi(null));
		}
	}
}