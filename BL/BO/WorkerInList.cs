using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// An abbreviated logical helper entity of worker
/// </summary>
public class WorkerInList
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public WorkerExperience Level { get; set; }
    public BO.TaskInWorker? CurrentTask { get; set; } = null;
    public override string ToString() => this.ToStringProperty();
}
