using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class TaskInWorker
{
    public int Id { get; init; }
    public required string Alias { get; set; }
}
