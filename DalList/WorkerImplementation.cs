
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Data;
using static DataSource;

internal class WorkerImplementation : IWorker
{
    /// <summary>
    /// Create a new worker
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Worker item)
    {
        foreach(Worker worker in DataSource.Workers)
        {
            if (worker.Id == item.Id)
                throw new DalAlreadyExistsException($"Worker with ID={item.Id} already exists");
        }
        if (item.Level == DO.WorkerExperience.Manager && Workers.FirstOrDefault(w => w.Level == DO.WorkerExperience.Manager) != null)
            throw new DalManagerException("There is already a manager for the project");
        DataSource.Workers.Add(item);
        return item.Id;
    }

    /// <summary>
    /// Delete a worker from the list
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Worker? worker = Read(id);
        if (worker == null)
            throw new DalDoesNotExistsException($"Worker with ID={id} doe's NOT exists");
        DataSource.Workers.Remove(worker);
    }

    /// <summary>
    /// Read a worker form the list by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    public Worker? Read(int id)
    {
        return DataSource.Workers.FirstOrDefault(worker => worker.Id == id) ?? throw new DalDoesNotExistsException($"Worker with ID={id} doe's NOT exists");
    }

    /// <summary>
    /// Read all the workers that meet the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Uapdate a worker by ID
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Worker item)
    {
        Worker? worker = Read(item.Id);
        if (worker == null)
            throw new DalDoesNotExistsException($"Worker with ID={item.Id} doe's NOT exists");

        if (item.Level == DO.WorkerExperience.Manager && worker.Level != DO.WorkerExperience.Manager && Workers.FirstOrDefault(w => w.Level == DO.WorkerExperience.Manager) != null) 
            throw new DalManagerException("There is already a manager for the project");

        Delete(item.Id);
        Create(item);
    }

    /// <summary>
    /// Read a worker that meets the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Worker? Read(Func<Worker, bool> filter) => Workers.FirstOrDefault(filter);

    public void Clear()
    {
        Workers.Clear();
    }
}
