
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class WorkerImplementation : IWorker
{
    public int Create(Worker item)
    {
        if (Read(item.Id) is not null)
            throw new Exception($"Worker with ID={item.Id} already exists");
        DataSource.Workers.Add(item);
        return item.Id;
    }
    
    public void Delete(int id)
    {
        Worker? worker = Read(id);
        if (worker == null)
            throw new Exception($"Worker with ID={id} doe's NOT exists");
        DataSource.Workers.Remove(worker);
    }

    public Worker? Read(int id)
    {
        return from item in DataSource.Workers
               where (item.Id) == id
               select item;
        //Worker? worker = DataSource.Workers.Find(worker => worker.Id == id);
        //if (worker != null)
        //    return worker;
        //return null;
    }

    public List<Worker> ReadAll()
    {
        return new List<Worker>(DataSource.Workers);
    }

    public void Update(Worker item)
    {
        if (Read(item.Id) == null)
            throw new Exception($"Worker with ID={item.Id} doe's NOT exists");
        Delete(item.Id);
        Create(item);
    }
}
