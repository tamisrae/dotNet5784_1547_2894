using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Task;

public class DependencyTask
{
    public int Id { get; init; }
    public required string Alias { get; set; }
    public required string Description { get; set; }
    public Status Status { get; set; }
    public required string IsDependent { get; set; }
}
