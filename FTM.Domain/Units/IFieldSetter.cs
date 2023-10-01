using System.Diagnostics.CodeAnalysis;

namespace FTM.Domain.Units;

public interface IFieldSetter<in T>
{ 
    public void Set(T entity, string fieldName, object value, Dictionary<string, object>? parameters = null);
}

public class FieldSetterAttribute : Attribute
{
    public FieldSetterAttribute(Type setterType)
    {
        SetterType = setterType;
    }

    public Type SetterType { get; }
}

