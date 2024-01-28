
using BlApi;
using BO;
using DalApi;
using DO;

namespace BlImplementation;

internal class WorkerImplementation : BlApi.IWorker
{
    private DalApi.IDal dal = DalApi.Factory.Get;

    public int Create(BO.Worker worker)
    {
        int level = (int)worker.Level;

        DO.Worker doWorker = new DO.Worker
           (worker.Id, (DO.WorkerExperience)level, worker.Email, worker.Cost, worker.Name);
        try
        {
            int idWorker = dal.Worker.Create(doWorker);
            return idWorker;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Student with ID={worker.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        try
        {
            dal.Worker.Delete(id);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Student with ID={id} already exists", ex);
        }
    }

    public WorkerInList GetDetailedTaskForWorker(int WorkerId, int TaskId)
    {
        throw new NotImplementedException();
    }

    public BO.Worker? Read(int id)
    {
        try
        {
            DO.Worker? doWorker = dal.Worker.Read(id);
            if (doWorker == null)
                throw new BO.BlDoesNotExistsException($"Student with ID={id} already exists");

            int level = (int)doWorker.Level;

            //BO.Worker worker = new BO.Worker
            //(doWorker.Id, (BO.WorkerExperience)level, doWorker.Email, doWorker.Cost, doWorker.Name);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Student with ID={id} already exists", ex);
        }
    }

    public IEnumerable<WorkerInList> ReadAll(Func<BO.Worker, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Worker worker)
    {
        throw new NotImplementedException();
    }
}
