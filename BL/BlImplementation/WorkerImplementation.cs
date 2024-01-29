
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
        if (worker.Name.IsEmptyString() || worker.Email.IsEmptyString() || worker.Id.IsGreaterThanZero() || worker.Cost.IsGreaterThanZero())
            throw new BlWorngValueException($"The worker has WORNG VALUE!");

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
            throw new BO.BlAlreadyExistsException($"Worker with ID={worker.Id} already exists", ex);
        }
    }

    public BO.TaskInWorker? CurrentTask(int id)
    {
        DO.Task? task = dal.Task.ReadAll().FirstOrDefault(item => item.WorkerId == id);
        BO.TaskInWorker? taskInWorker = null;

        if (task != null)
            taskInWorker = new BO.TaskInWorker { Id = id, Alias = task.Alias };
        return taskInWorker;
    }

    public void Delete(int id)
    {
        DO.Task? task = dal.Task.ReadAll().FirstOrDefault(item => item.WorkerId == id);

        if (task != null)
            throw new BO.BlWorkerInTaskException($"The worker is in the middle of executing a task");
        
        try
        {
            dal.Worker.Delete(id);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Worker with ID={id} already exists", ex);
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
                throw new BO.BlDoesNotExistsException($"Worker with ID={id} already exists");

            int level = (int)doWorker.Level;

            DO.Task? task = dal.Task.ReadAll().FirstOrDefault(item => item.WorkerId == id);
            BO.TaskInWorker? taskInWorker = null;

            if (task != null)
                taskInWorker = new BO.TaskInWorker { Id = id, Alias = task.Alias };

            return new BO.Worker()
            {
                Id = id,
                Level = (BO.WorkerExperience)level,
                Email = doWorker.Email,
                Cost = doWorker.Cost,
                Name = doWorker.Name,
                CurrentTask = taskInWorker
            };
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Worker with ID={id} already exists", ex);
        }
    }

    public IEnumerable<WorkerInList> ReadAll(Func<BO.Worker, bool>? filter = null)
    {
        return from item in dal.Worker.ReadAll()
               where filter!(Read(item.Id)!)
               select new BO.WorkerInList
               {
                   Name = item.Name,
                   Id = item.Id,
                   CurrentTask = CurrentTask(item.Id)
               };
    }

    public void Update(BO.Worker worker)
    {
        if (worker.Name.IsEmptyString() || worker.Email.IsEmptyString() || worker.Id.IsGreaterThanZero() || worker.Cost.IsGreaterThanZero())
            throw new BlWorngValueException($"The worker has WORNG VALUE!");

        int level = (int)worker.Level;

        DO.Worker doWorker = new DO.Worker
           (worker.Id, (DO.WorkerExperience)level, worker.Email, worker.Cost, worker.Name);

        try
        {
            dal.Worker.Update(doWorker);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Worker with ID={worker.Id} already exists", ex);
        }
    }
}
