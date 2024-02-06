using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Worker
{
    public int Id {  get; init; }
    public WorkerExperience Level { get; set; }
    public required string Email { get; set; }
    public double Cost { get; set; }
    public required string Name { get; set; }
    public BO.TaskInWorker? CurrentTask { get; set; } = null;
    public override string ToString() => this.ToStringProperty();
}
