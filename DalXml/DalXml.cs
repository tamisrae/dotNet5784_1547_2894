using DalApi;

namespace Dal;

/// <summary>
/// Stage 3
/// </summary>
sealed public class DalXml : IDal
{
    public IWorker Worker => new WorkerImplementation();

    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();
}
