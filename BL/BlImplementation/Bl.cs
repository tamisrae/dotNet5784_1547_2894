using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;
using BlApi;
//using DalApi;

internal class Bl : IBl
{
    public IWorker Worker => new WorkerImplementation();

    public ITask Task => new TaskImplementation();
}
