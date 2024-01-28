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
    public int Cost { get; set; }
    public required string Name { get; set; }
    public (int, string) CurrentTask { get; set; }

    public override string ToString() => this.ToStringProperty();
}
