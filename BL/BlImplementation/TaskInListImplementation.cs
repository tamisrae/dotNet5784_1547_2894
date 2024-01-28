
using BlApi;
using BO;

namespace BlImplementation;

internal class TaskInListImplementation : ITaskInList
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(TaskInList task)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public TaskInList? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TaskInList> ReadAll(Func<TaskInList, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(TaskInList task)
    {
        throw new NotImplementedException();
    }

    public void UpdateTheStatus(int id, Status newStatus)
    {
        throw new NotImplementedException();
    }
}
