using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;
using BlApi;
using BO;

//using DalApi;

//using DalApi;

//using DalApi;

internal class Bl : IBl
{
    private static DalApi.IDal dal = DalApi.Factory.Get;

    public IWorker Worker => new WorkerImplementation(this);
    public ITask Task => new TaskImplementation(this);
    public IUser User => new UserImplementation();

    private static DateTime s_Clock = DateTime.Now;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

    public void InitializeDB() => DalTest.Initialization.Do();

    public void ResetDB() => DalTest.Initialization.Reset();

    public BO.ProjectStatus GetProjectStatus()
    {
        BO.ProjectStatus projectStatus = BO.ProjectStatus.Scheduled;
        if (dal.StartProjectDate == null)
            projectStatus = BO.ProjectStatus.Unscheduled;

        if (dal.Task.ReadAll().FirstOrDefault(task => task.ScheduledDate != null && task.StartDate != null && task.StartDate < Clock) != null)
            projectStatus = BO.ProjectStatus.Execution;

        return projectStatus;
    }

    public DateTime? ScheduleDateOffer(BO.Task task)
    {
        DateTime? dateTime = null;
        IEnumerable<DO.Dependency>? dependencies = (from dependency in dal.Dependency.ReadAll()
                                                    where dependency.DependentTask == task.Id
                                                    select dependency);
        if (dependencies == null && dal.StartProjectDate != null)
            return dal.StartProjectDate;
        else
        {
            List<DO.Task>? tasksList = new();
            foreach (DO.Dependency dependency in dependencies!)
            {
                try
                {
                    DO.Task? doTask = dal.Task.Read(dependency.DependsOnTask);
                    if (doTask != null)
                        tasksList.Add(doTask);
                }
                catch (DO.DalDoesNotExistsException ex)
                {
                    throw new BO.BlDoesNotExistsException($"Task with ID={dependency.DependsOnTask} doe's NOT exists", ex);
                }
            }
            if (tasksList.Any())
            {
                if (tasksList.FirstOrDefault(t => t.ScheduledDate == null) != null)
                    throw new BlScheduledDateException($"You cannot enter scheduled date for This with ID={task.Id} task");
                else
                    dateTime = GetForeCastDate(tasksList.MaxBy(t => GetForeCastDate(t))!);
            }
        }
        return (DateTime)dateTime!;
    }

    public Status GetStatus(DO.Task task)
    {
        Status status;
        if (task.ScheduledDate == null)
            status = Status.Unscheduled;
        else if (task.ScheduledDate != null && (task.StartDate == null || task.StartDate > Clock))
            status = Status.Scheduled;
        else if (task.StartDate != null && task.StartDate <= Clock && (task.CompleteDate == null || task.CompleteDate > Clock))
            status = Status.OnTrack;
        else
            status = Status.Done;

        return status;
    }

    public DateTime? GetForeCastDate(DO.Task task)
    {
        DateTime? startDate;
        if (task.StartDate != null && task.StartDate > task.ScheduledDate)
            startDate = task.StartDate;
        else
            startDate = task.ScheduledDate;

        DateTime? foreCastDate = startDate + task.RequiredEffortTime;
        return foreCastDate;
    }

    public bool AllowedTask(int workerId, DO.Task task)
    {
        if (task.WorkerId != null && workerId != task.WorkerId)//if another worker work on this task
            return false;
        DO.Worker? worker = dal.Worker.Read(workerId);
        if (worker != null && worker.Level != task.Complexity)//if the level of the worker does not fit to the complexity of the task
            return false;

        IEnumerable<DO.Task>? tasks = (from dependency in dal.Dependency.ReadAll()
                                       where dependency.DependentTask == task.Id
                                       select dal.Task.Read(dependency.DependsOnTask));

        DO.Task? tempTask = tasks.FirstOrDefault(t => GetStatus(t) != BO.Status.Done);//If one of the tasks that the task depends on has not finished
        if (tempTask != null)
            return false;
        return true;
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
