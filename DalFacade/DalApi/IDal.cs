using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface IDal
    {
        IWorker Worker { get; }
        ITask Task { get; }
        IDependency Dependency { get; }
    }

}
