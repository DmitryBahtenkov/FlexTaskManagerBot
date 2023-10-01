namespace FTM.Domain.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string mes) : base(mes){}
    public BusinessException(string mes, Exception inner) : base(mes, inner){}
}

public class DuplicateException : BusinessException
{
    private Object _existing;
    public DuplicateException(object existing, string mes) : base(mes)
    {
        _existing = existing;
    }

    public T? GetExistingObject<T>()
    {
        return _existing is T t ? t : default;
    }
}

public class ValidationException : Exception
{
    public Dictionary<string, string?> PropertyErrors { get; }

    public ValidationException(Dictionary<string, string?> propertyErrors)
    {
        PropertyErrors = propertyErrors;
    }
}