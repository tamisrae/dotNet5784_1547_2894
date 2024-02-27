using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// Logical auxiliary entity
/// </summary>
public class TaskInWorker
{
    public int Id { get; set; }
    public required string Alias { get; set; }
    public override string ToString() => this.ToStringProperty();
}
