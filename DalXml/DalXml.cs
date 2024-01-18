using DalApi;

namespace Dal;

sealed public class DalXml : IDal
{
    public IWorker Worker => new WorkerImplementation();

    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();
}
