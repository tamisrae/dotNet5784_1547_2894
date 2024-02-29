using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;
using BlApi;
//using DalApi;

//using DalApi;

internal class Bl : IBl
{
    public IWorker Worker => new WorkerImplementation();

    public ITask Task => new TaskImplementation();

    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();

    //private DalApi.IDal dal = DalApi.Factory.Get;
    //public void SetStartProjectDate(DateTime? startDate) => dal.StartProjectDate(startDate);

    public BO.ProjectStatus ProjectStatusPL()
    {
        return IBl.GetProjectStatus();
    }
}
