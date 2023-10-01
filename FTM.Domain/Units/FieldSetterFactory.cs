using System.Reflection;

namespace FTM.Domain.Units;

public class FieldSetterFactory<T>
{
    private readonly IEnumerable<IFieldSetter<T>> _setters;

    public FieldSetterFactory(IEnumerable<IFieldSetter<T>> setters)
    {
        _setters = setters;
    }
    
    public IFieldSetter<T> CreateSetter(string fieldName)
    {
        var attribute = typeof(T).GetProperty(fieldName)?.GetCustomAttribute<FieldSetterAttribute>();  
        return attribute is null 
            ? new DefaultFieldSetter<T>() 
            : _setters.First(x => x.GetType() == attribute.SetterType);
    }
}