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

    private static DateTime s_Clock = DateTime.Now;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();


    private static DalApi.IDal dal = DalApi.Factory.Get;
    public BO.ProjectStatus ProjectStatusPL()
    {
        return IBl.GetProjectStatus();
    }

    public void StartProjectDate(DateTime projectDate) => dal.StartProjectDate = projectDate;

    public void AdvanceTimeByYear()
    {
        Clock= Clock.AddYears(1);
    }

    public void AdvanceTimeByDay()
    {
        Clock = Clock.AddDays(1);
    }

    public void AdvanceTimeByMonth()
    {
        Clock = Clock.AddMonths(1);
    }

    public void AdvanceTimeByHour()
    {
        Clock = Clock.AddHours(1);
    }

    public void ResetTime()
    {
        Clock= DateTime.Now;
    }
}
