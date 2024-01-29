using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BO;

public class Task
{
    public int Id { get; init; }
    public required string Alias { get; set; }
    public required string Description { get; set; }
    public Status Status { get; set; }
    public BO.WorkerInTask? WorkOnTask { get; set; } = null;
    public List<TaskInList>? Dependencies { get; set; } = null;
    public DateTime CreatedAtDate {  get; set; }
    public DateTime? ScheduledDate { get; set; } = null;
    public DateTime? StartDate { get; set; } = null;
    public DateTime? CompleteDate { get; set; } = null;
    public DateTime? ForeCastDate { get; set; } = null;
    public TimeSpan? RequiredEffortTime { get; set; } = null;
    public string? Deliverables { get; set; } = null;
    public string? Remarks { get; set; } = null;
    public DO.WorkerExperience? Complexity { get; set; } = null;
    public int? WorkerId { get; set; } = null;

    public override string ToString() => this.ToStringProperty();
}
