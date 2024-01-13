
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using static DataSource;

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
        return DataSource.Workers.FirstOrDefault(worker => worker.Id == id);
        //Worker? worker = DataSource.Workers.Find(worker => worker.Id == id);
        //if (worker != null)
        //    return worker;
        //return null;
    }

    //public List<Worker> ReadAll()
    //{
    //    return new List<Worker>(DataSource.Workers);
    //}

    public IEnumerable<Worker> ReadAll(Func<Worker, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Workers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Workers
               select item;
    }

    public void Update(Worker item)
    {
        if (Read(item.Id) == null)
            throw new Exception($"Worker with ID={item.Id} doe's NOT exists");
        Delete(item.Id);
        Create(item);
    }

    public Worker? Read(Func<Worker, bool> filter) => Workers.FirstOrDefault(filter);
}
