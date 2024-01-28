using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IBl
{
    public IWorker Worker { get; }
    public ITask Task { get; }
    public IWorkerInList WorkerInList { get; }
    public ITaskInList TaskInList { get; }
}
