using BO;


namespace BlApi;

public interface IBl
{
    public IWorker Worker { get; }
    public ITask Task { get; }
    public IUser User { get; }


    private static DalApi.IDal dal = DalApi.Factory.Get;

    public BO.ProjectStatus GetProjectStatus();

    public DateTime? ScheduleDateOffer(BO.Task task);

    public Status GetStatus(DO.Task task);

    public DateTime? GetForeCastDate(DO.Task task);

    public bool AllowedTask(int workerId, DO.Task task);

    public void InitializeDB();

    public void ResetDB();

    public void StartProjectDate(DateTime projectDate);

    public DateTime Clock { get; }
    public void AdvanceTimeByYear();

    public void AdvanceTimeByDay();

    public void AdvanceTimeByMonth();

    public void AdvanceTimeByHour();

    public void ResetTime();
};




