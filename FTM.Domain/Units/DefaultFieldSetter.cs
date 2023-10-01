namespace FTM.Domain.Units;

public class DefaultFieldSetter<T> : IFieldSetter<T>
{
    public void Set(T entity, string fieldName, object value, Dictionary<string, object>? parameters = null)
    {
        var property = typeof(T).GetProperty(fieldName);

        property?.SetValue(entity, value);
    }
}
