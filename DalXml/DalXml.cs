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

    public DateTime? StartProjectDate { get { return Config.GetProjectDate("Start project date"); } set { Config.SetProjectDate("Start project date", value); } }
    public DateTime? EndProjectDate { get { return Config.GetProjectDate("End project date"); } set { Config.SetProjectDate("End project date", value); } }
}
