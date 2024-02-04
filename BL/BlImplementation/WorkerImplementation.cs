
using BlApi;
using BO;
using DalApi;
using DO;
using System.ComponentModel.Design;

namespace BlImplementation;

internal class WorkerImplementation : BlApi.IWorker
{
    private DalApi.IDal dal = DalApi.Factory.Get;

    public int Create(BO.Worker worker)
    {
        if (worker.Name.IsEmptyString() || worker.Email.IsEmptyString() || worker.Id.IsGreaterThanZero() || worker.Cost.IsGreaterThanZero())
            throw new BlWorngValueException($"The worker has WORNG VALUE!");
        //BO.ProjectStatus status = IBl.GetProjectStatus();
        

        if (worker.CurrentTask != null)
        {
            try
            {
                DO.Task? task = dal.Task.Read(worker.CurrentTask.Id);
                if (task != null)
                {
                    if (task.WorkerId != null)
                        throw new BlTaskInWorkerException("Another worker is already doing this task");
                    else
                    {
                        task = task with { WorkerId = task.WorkerId };
                        dal.Task.Update(task);
                    }
                }
            }
            catch (DO.DalDoesNotExistsException ex)
            {
                throw new BO.BlDoesNotExistsException($"Worker with ID={worker.CurrentTask.Id} doe's NOT exists", ex);
            }
        }

        DO.Worker doWorker = new DO.Worker
           (worker.Id, (DO.WorkerExperience)((int)worker.Level), worker.Email, worker.Cost, worker.Name);
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

    public void Delete(int id)
    {
        try
        {
            //BO.Worker? worker = Read(id);
            //    BO.Task? task = null;
            //if (worker!=null && worker.CurrentTask != null)
            //     task = BlApi.ITask.Read(worker.CurrentTask.Id);
            DO.Task? task = dal.Task.ReadAll().FirstOrDefault(item => item.WorkerId == id);

            if (task != null)
                throw new BO.BlWorkerInTaskException($"The worker is in the middle of executing a task");

            dal.Worker.Delete(id);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Worker with ID={id} doe's NOT exists", ex);
        }
    }

    public BO.Worker? Read(int id)
    {
        try
        {
            DO.Worker? doWorker = dal.Worker.Read(id);
            if (doWorker == null)
                throw new BO.BlDoesNotExistsException($"Worker with ID={id} doe's NOT exists");

            int level = (int)doWorker.Level;

            DO.Task? task = dal.Task.ReadAll().FirstOrDefault(item => item.WorkerId == id);
            BO.TaskInWorker? taskInWorker = null;

            if (task != null)
                taskInWorker = new BO.TaskInWorker { Id = id, Alias = task.Alias };

            return new BO.Worker()
            {
                Id = id,
                Level = (BO.WorkerExperience)((int)doWorker.Level),
                Email = doWorker.Email,
                Cost = doWorker.Cost,
                Name = doWorker.Name,
                CurrentTask = taskInWorker
            };
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Worker with ID={id} doe's NOT exists", ex);
        }
    }

    public IEnumerable<WorkerInList> ReadAll(Func<BO.Worker, bool>? filter = null)
    {
        if (filter == null)
        {
            return from item in dal.Worker.ReadAll()
                   select new BO.WorkerInList
                   {
                       Name = item.Name,
                       Id = item.Id,
                       Level = (BO.WorkerExperience)((int)item.Level),
                       CurrentTask = CurrentTask(item.Id)
                   };
        }
        else
        {
            return from item in dal.Worker.ReadAll()
                   where filter!(Read(item.Id)!)
                   select new BO.WorkerInList
                   {
                       Name = item.Name,
                       Id = item.Id,
                       Level = (BO.WorkerExperience)((int)item.Level),
                       CurrentTask = CurrentTask(item.Id)
                   };
        }
    }

    public void Update(BO.Worker worker)
    {
        if (worker.Name.IsEmptyString() || worker.Email.IsEmptyString() || worker.Id.IsGreaterThanZero() || worker.Cost.IsGreaterThanZero())
            throw new BlWorngValueException($"The worker has WORNG VALUE!");

        DO.Worker doWorker = new DO.Worker
           (worker.Id, (DO.WorkerExperience)((int)worker.Level), worker.Email, worker.Cost, worker.Name);

        try
        {
            dal.Worker.Update(doWorker);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Worker with ID={worker.Id} doe's NOT exists", ex);
        }
    }

    private BO.TaskInWorker? CurrentTask(int id)
    {
        DO.Task? task = dal.Task.ReadAll().FirstOrDefault(item => item.WorkerId == id);
        BO.TaskInWorker? taskInWorker = null;


        if (task != null)
            taskInWorker = new BO.TaskInWorker { Id = id, Alias = task.Alias };
        return taskInWorker;
    }
}
