using System.Collections.Generic;

namespace FubuValidation.StructureMap
{
	public class ValidationRegistrationRunner
	{
		private readonly ValidationGraph _graph;
		private readonly IEnumerable<IValidationRegistration> _registrations;

		public ValidationRegistrationRunner(ValidationGraph graph, IEnumerable<IValidationRegistration> registrations)
		{
			_graph = graph;
			_registrations = registrations;
		}

		public void Run()
		{
			_registrations.Each(x => _graph.Import(x));
		}
	}
}