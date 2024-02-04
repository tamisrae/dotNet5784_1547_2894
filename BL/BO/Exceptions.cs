
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

[Serializable]
public class BlWorngValueException : Exception
{
    public BlWorngValueException(string? message) : base(message) { }
}

[Serializable]
public class BlWorkerInTaskException : Exception
{
    public BlWorkerInTaskException(string? message) : base(message) { }
}

[Serializable]
public class BlCantUpdateException : Exception
{
    public BlCantUpdateException(string? message) : base(message) { }
}

[Serializable]
public class BlCantDeleteException : Exception
{
    public BlCantDeleteException(string? message) : base(message) { }
}

[Serializable]
public class BlTaskInWorkerException : Exception
{
    public BlTaskInWorkerException(string? message) : base(message) { }
}

