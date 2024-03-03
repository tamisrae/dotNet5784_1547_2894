using BO;


namespace BlApi;

public interface IBl
{
    public IWorker Worker { get; }
    public ITask Task { get; }
    public IUser User { get; }


    private static DalApi.IDal dal = DalApi.Factory.Get;

    /// <summary>
    /// This function returns the status of the project
    /// </summary>
    /// <returns></returns>
    public static BO.ProjectStatus GetProjectStatus()
    {
        BO.ProjectStatus projectStatus = BO.ProjectStatus.Scheduled;
        if (dal.StartProjectDate == null)
            projectStatus = BO.ProjectStatus.Unscheduled;

        if (dal.Task.ReadAll().FirstOrDefault(task => task.ScheduledDate != null && task.StartDate != null && task.StartDate < DateTime.Now) != null)
            projectStatus = BO.ProjectStatus.Execution;

        return projectStatus;
    }

    /// <summary>
    /// The function returns a suggestion for a task scheduled date
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    /// <exception cref="BlScheduledDateException"></exception>
    public static DateTime? ScheduleDateOffer(BO.Task task)
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
                    dateTime = tasksList.MaxBy(t => t.GetForeCastDate())!.GetForeCastDate();
            }
        }
        return (DateTime)dateTime!;
    }

    public void InitializeDB();
    public void ResetDB();

    public BO.ProjectStatus ProjectStatusPL();

    public void StartProjectDate(DateTime projectDate);
};




