using System;

namespace FubuMVC.Validation
{
    public interface IFubuContinuationModelDescriptor
    {
        Type DescribeModelFor(ValidationFailureContext context);
    }
}