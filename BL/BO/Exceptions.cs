
namespace BO;

[Serializable]
public class BlDoesNotExistsException : Exception
{
    public BlDoesNotExistsException(string? message) : base(message) { }
    public BlDoesNotExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}


[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}

