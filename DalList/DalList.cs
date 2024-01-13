using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    using DalApi;
    sealed public class DalList : IDal
    {
        public IWorker Worker => new WorkerImplementation();

        public ITask Task => new TaskImplementation();

        public IDependency Dependency => new DependencyImplementation();
    }
}
