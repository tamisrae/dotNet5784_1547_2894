using DalApi;
using System.Diagnostics;

namespace Dal;

/// <summary>
/// Stage 3
/// </summary>
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
 

    public IWorker Worker => new WorkerImplementation();
    public ITask Task => new TaskImplementation();
    public IDependency Dependency => new DependencyImplementation();
    public IUser User => new UserImplementation();

    public DateTime? StartProjectDate { get { return Config.GetProjectDate("StartProjectDate"); } set { Config.SetProjectDate("StartProjectDate", value); } }
    public DateTime? EndProjectDate { get { return Config.GetProjectDate("EndProjectDate"); } set { Config.SetProjectDate("EndProjectDate", value); } }

}
