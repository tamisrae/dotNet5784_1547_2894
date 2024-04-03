using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class GantTask
{
    public int Id {  get; set; }
    public string? Alias { get; set; }
    public BO.Status Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime CompleteDate { get; set; }
    public int? WorkerId { get; set; }
    public string? WorkerName { get; set; }
    public IEnumerable<int>? DependentTasks { get; set;}
}
