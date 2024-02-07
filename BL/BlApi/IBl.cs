using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DalApi;


namespace BlApi;

public interface IBl
{
    public IWorker Worker { get; }
    public ITask Task { get; }

    //public static DateTime? StartProjectDate { get; set {dal. }
    //public static DateTime? EndProjectDate { get; set; }

    private static DalApi.IDal dal = DalApi.Factory.Get;

    public static BO.ProjectStatus GetProjectStatus()
    {
        BO.ProjectStatus projectStatus = BO.ProjectStatus.Scheduled;
        if (dal.StartProjectDate == null)
            projectStatus = BO.ProjectStatus.Unscheduled;

        if (dal.Task.ReadAll().FirstOrDefault(task => task.ScheduledDate != null && task.StartDate != null && task.StartDate < DateTime.Now) != null)
            projectStatus = BO.ProjectStatus.Execution;

        return projectStatus;
    }

public  DateTime? ScheduleDateOffer(BO.Task task)
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
                DO.Task? task1 = dal.Task.Read(dependency.DependsOnTask);
                if (task1 != null)
                    tasksList.Add(task1);
            }
            if (tasksList.Count == 0)
            {
                if (tasksList.FirstOrDefault(task => task.StartDate == null) != null)
                    throw new BlScheduledDateException("You cannot enter scheduled date for this task");
                else
                    dateTime = tasksList.MaxBy(task => task.GetForeCastDate())!.GetForeCastDate();
            }
        }
        return (DateTime)dateTime!;
    }
};




