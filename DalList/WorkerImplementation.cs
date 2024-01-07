
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class WorkerImplementation : IWorker
{
    public int Create(Worker item)
    {
        Worker? cc = Workers.(cc => cc.Id == item.Id);
        throw new NotImplementedException();
    }
    
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Worker? Read(int id)
    {
        throw new NotImplementedException();
    }

    public List<Worker> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Worker item)
    {
        throw new NotImplementedException();
    }
}
