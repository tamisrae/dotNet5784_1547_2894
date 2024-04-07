using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    using DalApi;
    sealed internal class DalList : IDal
    {
        public static IDal Instance { get; } = new DalList(); 
        private DalList() { }

        public IWorker Worker => new WorkerImplementation();
        public ITask Task => new TaskImplementation();
        public IDependency Dependency => new DependencyImplementation();
        public IUser User => new UserImplementation();

        public  DateTime? StartProjectDate { get { return DataSource.Config.StartProjectDate; } set { DataSource.Config.StartProjectDate = value; } }
        public  DateTime? EndProjectDate { get { return DataSource.Config.StartProjectDate; } set { DataSource.Config.StartProjectDate = value; } }
    }
}
