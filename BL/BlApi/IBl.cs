using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IBl
{
    public IWorker Worker { get; }
    public ITask Task { get; }

    public static DateTime? StartProjectDate { get; set; } = null;
    public static DateTime? EndProjectDate { get; set; } = null;

    public BO.ProjectStatus GetProjectStatus()
    {
        BO.ProjectStatus projectStatus = BO.ProjectStatus.Execution;
        if (StartProjectDate == null)
            projectStatus = BO.ProjectStatus.Unscheduled;

        IEnumerable<TaskInList> listOfTask = Task.ReadAll();
        foreach (TaskInList taskInList in listOfTask)
        {
            BO.Task? task = Task.Read(taskInList.Id);
            if (task != null && task.ScheduledDate != null && (task.StartDate == null || task.StartDate > DateTime.Now))
                projectStatus = BO.ProjectStatus.Scheduled;
        }
        return projectStatus;
    }
};
