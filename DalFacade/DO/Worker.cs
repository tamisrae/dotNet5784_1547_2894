
using System.Reflection.Emit;
using System.Xml.Linq;

namespace DO;
/// <summary>
///  worker Entity represents a worker with all its props
/// </summary>
/// <param name="Id"> Personal unique ID of the worker (as in national id card) </param>
/// <param name="Level"> represents the experience of the worker </param>
/// <param name="Email"> the worker's email </param> 
/// <param name="Cost"> the worker's cost per hour </param>
/// <param name="Name"> Private Name of the student </param>
public record Worker
(
    int Id,
    DO.WorkerExperience Level,
    string? Email = null,
    double? Cost = null,
    string? Name = null
 )
{
    public Worker() : this(0, 0) { }//empty ctor
  
}

