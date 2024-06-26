﻿
namespace DO;

[Serializable]
public class DalDoesNotExistsException : Exception
{
    public DalDoesNotExistsException(string? message) : base(message) { }
}

[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}

[Serializable]
public class DalWorngValueException : Exception//If the user entered worng value to the switch/id/cost per hour/level/complexity
{
    public DalWorngValueException(string? message) : base(message) { }
}

[Serializable]
public class DalXMLFileLoadCreateException : Exception
{ 
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}

[Serializable]
public class DalManagerException : Exception
{
    public DalManagerException(string? message) : base(message) { }
}