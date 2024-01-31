
using System.Reflection.Emit;
using System.Xml.Linq;

namespace DO;
/// <summary>
/// A dependency entity represents a task's dependencies on other tasks
/// </summary>
/// <param name="Id"> Personal unique ID of the dependency </param>
/// <param name="DependentTask"> ID number of pending task </param>
/// <param name="DependsOnTask"> Previous task ID number </param>
public record Dependency
(
    int Id,
    int DependentTask,
    int DependsOnTask
)
{
    public Dependency() : this(0, 0, 0) { }//empty ctor
}
//1 tluy 2
//2 tluy 3
//id=2
//3
//1