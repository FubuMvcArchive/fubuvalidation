using System;

namespace FubuMVC.Validation
{
    public interface IInputModelResolver
    {
        object Resolve(Type destinationType, Type sourceType, object source);
    }
}