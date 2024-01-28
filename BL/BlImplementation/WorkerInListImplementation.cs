
using BlApi;
using BO;

namespace BlImplementation;

internal class WorkerInListImplementation : IWorkerInList
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(WorkerInList worker)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public WorkerInList GetDetailedTaskForWorker(int WorkerId, int TaskId)
    {
        throw new NotImplementedException();
    }

    public WorkerInList? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<WorkerInList> ReadAll(Func<WorkerInList, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(WorkerInList worker)
    {
        throw new NotImplementedException();
    }
}
