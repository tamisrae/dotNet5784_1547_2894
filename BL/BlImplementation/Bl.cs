using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;
using BlApi;
//using DalApi;

//using DalApi;

//using DalApi;

internal class Bl : IBl
{
    public IWorker Worker => new WorkerImplementation();
    public ITask Task => new TaskImplementation();
    public IUser User => new UserImplementation();


    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();


    private static DalApi.IDal dal = DalApi.Factory.Get;
    public BO.ProjectStatus ProjectStatusPL()
    {
        return IBl.GetProjectStatus();
    }

    public void StartProjectDate(DateTime projectDate) => dal.StartProjectDate = projectDate;
}
