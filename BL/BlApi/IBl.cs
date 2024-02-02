using System;
using System.Collections.Generic;
using System.Linq;
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
        //tasks????
        return projectStatus;
    }
}
