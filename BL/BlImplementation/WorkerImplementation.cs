using BlApi;
using BO;

namespace BlImplementation;

internal class WorkerImplementation : IWorker
{
    private DalApi.IDal dal = DalApi.Factory.Get;
    private readonly IBl _bl;
    internal WorkerImplementation(IBl bl) => _bl = bl;


    public void Clear()
    {
        dal.Worker.Clear();
    }

    /// <summary>
    /// This function create a new logic worker
    /// </summary>
    /// <param name="worker"></param>
    /// <returns></returns>
    /// <exception cref="BlWorngValueException"></exception>
    /// <exception cref="BlTaskInWorkerException"></exception>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.Worker worker)
    {
        if (worker.Name.IsEmptyString() || worker.Email.IsEmptyString() || worker.Id.IsGreaterThanZero() || worker.Cost.IsGreaterThanZero())
            throw new BlWorngValueException("The worker has WORNG VALUE!");
        if (worker.Level == BO.WorkerExperience.Manager && dal.Worker.ReadAll().FirstOrDefault(w => w.Level == DO.WorkerExperience.Manager) != null)
            throw new BlManagerException("There is already a manager for the project");

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

    /// <summary>
    /// This function delete a logic worker
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlWorkerInTaskException"></exception>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    public void Delete(int id)
    {
        try
        {
            IEnumerable<DO.Task>? tasks = from DO.Task task in dal.Task.ReadAll()
                                          where task.WorkerId == id
                                          select task;
            if (!tasks.Any())//Check if the worker is registered for tasks
            {
                dal.Worker.Delete(id);
                dal.User.Delete(id);
            }
            else
                throw new BO.BlWorkerInTaskException($"You cannot delete worker with ID={id}");
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Worker with ID={id} doe's NOT exists", ex);
        }
    }

    /// <summary>
    /// This function read logic worker
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    public BO.Worker? Read(int id)
    {
        try
        {
            DO.Worker? doWorker = dal.Worker.Read(id);
            if (doWorker == null)
                throw new BO.BlDoesNotExistsException($"Worker with ID={id} doe's NOT exists");

            int level = (int)doWorker.Level;

            return new BO.Worker
            {
                Id = id,
                Level = (BO.WorkerExperience)((int)doWorker.Level),
                Email = doWorker.Email,
                Cost = doWorker.Cost,
                Name = doWorker.Name,
                CurrentTask = CurrentTask(id)
            };
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Worker with ID={id} doe's NOT exists", ex);
        }
    }

    /// <summary>
    /// This function returns all the workers from the data source
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Worker> ReadAll(Func<BO.Worker, bool>? filter = null)
    {
        if (filter == null)
        {
            IEnumerable<BO.Worker> list = from item in dal.Worker.ReadAll()//without filter
                                          select new BO.Worker
                                          {
                                              Id = item.Id,
                                              Level = (BO.WorkerExperience)((int)item.Level),
                                              Email = item.Email,
                                              Cost = item.Cost,
                                              Name = item.Name,
                                              CurrentTask = CurrentTask(item.Id)
                                          };
            return list.OrderBy(worker => worker.Level);
        }
        else
        {
            IEnumerable<BO.Worker> list = from item in dal.Worker.ReadAll()//with filter
                                          where filter!(Read(item.Id)!)
                                          select new BO.Worker
                                          {
                                              Id = item.Id,
                                              Level = (BO.WorkerExperience)((int)item.Level),
                                              Email = item.Email,
                                              Cost = item.Cost,
                                              Name = item.Name,
                                              CurrentTask = CurrentTask(item.Id)
                                          };
            return list.OrderBy(worker => worker.Level);
        }
    }

    /// <summary>
    /// This function update logic worker
    /// </summary>
    /// <param name="worker"></param>
    /// <exception cref="BlWorngValueException"></exception>
    /// <exception cref="BlTaskInWorkerException"></exception>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    public void Update(BO.Worker worker)
    {
        try
        {
            if (worker.Name.IsEmptyString() || worker.Email.IsEmptyString() || worker.Id.IsGreaterThanZero() || worker.Cost.IsGreaterThanZero())
                throw new BlWorngValueException($"The worker has WORNG VALUE!");

            DO.Worker? doWorker = dal.Worker.Read(worker.Id);
            if (doWorker != null && worker.Level == BO.WorkerExperience.Manager && doWorker.Level != DO.WorkerExperience.Manager && dal.Worker.ReadAll().FirstOrDefault(w => w.Level == DO.WorkerExperience.Manager) != null)
                throw new BlManagerException("There is already a manager for the project");

            if (worker.CurrentTask != null)
            {
                try
                {
                    DO.Task? task = dal.Task.Read(worker.CurrentTask.Id);
                    if (task != null)
                    {
                        if (!(_bl.AllowedTask(worker.Id, task)))//check if the worker can take the task
                            throw new BlTaskInWorkerException("You cannot take this task");

                        BO.TaskInWorker? taskInWorker = CurrentTask(worker.Id);//check if the worker in the middle of another task
                        if (taskInWorker != null && taskInWorker.Id != worker.CurrentTask.Id)
                            throw new BlTaskInWorkerException("The worker is in the middle of another task");

                        task = task with { WorkerId = worker.Id };
                        dal.Task.Update(task);
                    }

                }
                catch (DO.DalDoesNotExistsException ex)
                {
                    throw new BO.BlDoesNotExistsException($"Task with ID={worker.CurrentTask.Id} doe's NOT exists", ex);
                }
            }

            DO.Worker WorkerToUpdate = new DO.Worker
               (worker.Id, (DO.WorkerExperience)((int)worker.Level), worker.Email, worker.Cost, worker.Name);


            dal.Worker.Update(WorkerToUpdate);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Worker with ID={worker.Id} doe's NOT exists", ex);
        }
    }

    /// <summary>
    /// This function returns the current task of a worker
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private BO.TaskInWorker? CurrentTask(int id)
    {
        IEnumerable<DO.Task>? tasks = dal.Task.ReadAll().Where(item => item.WorkerId == id);
        DO.Task? task = tasks.FirstOrDefault(t => _bl.GetStatus(t) == BO.Status.OnTrack);

        BO.TaskInWorker? taskInWorker = null;
        if (task != null)
            taskInWorker = new BO.TaskInWorker { Id = task.Id, Alias = task.Alias };
        return taskInWorker;
    }
}
