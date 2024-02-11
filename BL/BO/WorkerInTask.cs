using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// Logical auxiliary entity
/// </summary>
public class WorkerInTask
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public override string ToString() => this.ToStringProperty();
}
