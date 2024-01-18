
using System;
using System.Reflection.Emit;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;

namespace DO;
/// <summary>
///  Task Entity represents a task with all its props
/// </summary>
/// <param name="Id"> Personal unique ID of the task </param>
/// <param name="Compelexity"> The difficulty level of the task </param>
/// <param name="WorkerId"> ID number of the employee assigned to the task </param>
/// <param name="RequiredEffortTime"> The amount of time required to perform the task </param>
/// <param name="StartDate"> start date of work on the task </param>
/// <param name="ScheduledDate"> Planned date for work to begin </param>
/// <param name="Deadlinedate"> Possible final end date </param>
/// <param name="CompleteDate"> Actual end date </param>
/// <param name="Deliverables"> The final deliverable </param>
/// <param name="Remarks"> Remarks </param>
/// <param name="Alias"> The task's alias </param>
/// <param name="Description"> task description </param>
/// <param name="CreatedAtDate"> Task creation date </param>
/// <param name="IsMilestone"> Is there a milestone? </param>
public record Task
(
    string Alias,
    string Description,
    DateTime CreatedAtDate,
    bool IsMilestone,
    int Id,
    DO.WorkerExperience? Complexity = null,
    int? WorkerId = null,
    TimeSpan? RequiredEffortTime = null,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? Deadlinedate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null
)
{
    public Task() : this("", "", DateTime.Now, false, 0) { }//empty ctor    
}

