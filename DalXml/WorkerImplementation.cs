
namespace Dal;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;

internal class WorkerImplementation : IWorker
{
    readonly string s_workers_xml = "workers";

    public void Clear()
    {
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);
        workers.Clear();
        XMLTools.SaveListToXMLSerializer<DO.Worker>(workers, s_workers_xml);
    }

    public int Create(Worker item)
    {
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);
        foreach (Worker worker in workers)
        {
            if (worker.Id == item.Id)
                throw new DalAlreadyExistsException($"Worker with ID={item.Id} already exists");
        }
        workers.Add(item);
        XMLTools.SaveListToXMLSerializer<DO.Worker>(workers, s_workers_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);
        Worker? worker = Read(id);
        if (worker == null)
            throw new DalDoesNotExistException($"Worker with ID={id} doe's NOT exists");
        workers.Remove(worker);
        XMLTools.SaveListToXMLSerializer<DO.Worker>(workers, s_workers_xml);
    }

    public Worker? Read(int id)
    {
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);

        return workers.FirstOrDefault(worker => worker.Id == id) ?? throw new DalDoesNotExistException($"Worker with ID={id} doe's NOT exists");
    }

    public Worker? Read(Func<Worker, bool> filter)
    {
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);

        return workers.FirstOrDefault(filter);
    }

    public IEnumerable<Worker?> ReadAll(Func<Worker, bool>? filter = null)
    {
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);

        if (filter != null)
        {
            return from item in workers
                   where filter(item)
                   select item;
        }
        return from item in workers
               select item;
    }

    public void Update(Worker item)
    {
        if (Read(item.Id) == null)
            throw new DalDoesNotExistException($"Worker with ID={item.Id} doe's NOT exists");
        Delete(item.Id);
        Create(item);
    }
}

