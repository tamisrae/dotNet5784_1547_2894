using BO;

namespace BlImplementation;

internal class WorkerImplementation : BlApi.IWorker
{
    private DalApi.IDal dal = DalApi.Factory.Get;

    public int Create(BO.Worker worker)
    {
        if (worker.Name.IsEmptyString() || worker.Email.IsEmptyString() || worker.Id.IsGreaterThanZero() || worker.Cost.IsGreaterThanZero())
            throw new BlWorngValueException($"The worker has WORNG VALUE!");

        if (worker.CurrentTask != null)
        {
            try
            {
                DO.Task? task = dal.Task.Read(worker.CurrentTask.Id);
                if (task != null)
                {
                    if (!(Tools.AllowedTask(worker.Id, task)))
                        throw new BlTaskInWorkerException("You cannot take this task");
                    else
                    {
                        task = task with { WorkerId = worker.Id };
                        dal.Task.Update(task);
                    }
                }

            }
            catch (DO.DalDoesNotExistsException ex)
            {
                throw new BO.BlDoesNotExistsException($"Task with ID={worker.CurrentTask.Id} doe's NOT exists", ex);
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
            IEnumerable<DO.Task>? tasks = from DO.Task task in dal.Task.ReadAll()
                                          where task.WorkerId == id
                                          select task;
            if (!tasks.Any())
                dal.Worker.Delete(id);
            else
                throw new BO.BlWorkerInTaskException($"You cannot delete this worker");
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
            BO.TaskInWorker? currentTask = CurrentTask(id);

            return new BO.Worker
            {
                Id = id,
                Level = (BO.WorkerExperience)((int)doWorker.Level),
                Email = doWorker.Email,
                Cost = doWorker.Cost,
                Name = doWorker.Name,
                CurrentTask = currentTask
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
            IEnumerable<BO.WorkerInList> list = from item in dal.Worker.ReadAll()
                                                select new BO.WorkerInList
                                                {
                                                    Name = item.Name,
                                                    Id = item.Id,
                                                    Level = (BO.WorkerExperience)((int)item.Level),
                                                    CurrentTask = CurrentTask(item.Id)
                                                };
            return list.OrderBy(worker => worker.Level);
        }
        else
        {
            IEnumerable<BO.WorkerInList> list = from item in dal.Worker.ReadAll()
                                                where filter!(Read(item.Id)!)
                                                select new BO.WorkerInList
                                                {
                                                    Name = item.Name,
                                                    Id = item.Id,
                                                    Level = (BO.WorkerExperience)((int)item.Level),
                                                    CurrentTask = CurrentTask(item.Id)
                                                };
            return list.OrderBy(worker => worker.Level);
        }
    }

    public void Update(BO.Worker worker)
    {
        if (worker.Name.IsEmptyString() || worker.Email.IsEmptyString() || worker.Id.IsGreaterThanZero() || worker.Cost.IsGreaterThanZero())
            throw new BlWorngValueException($"The worker has WORNG VALUE!");

        if (worker.CurrentTask != null)
        {
            try
            {
                DO.Task? task = dal.Task.Read(worker.CurrentTask.Id);
                if (task != null)
                {
                    if (!(Tools.AllowedTask(worker.Id, task)))
                        throw new BlTaskInWorkerException("You cannot take this task");

                    BO.TaskInWorker? taskInWorker = CurrentTask(worker.Id);
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
        IEnumerable<DO.Task>? tasks = dal.Task.ReadAll().Where(item => item.WorkerId == id);
        DO.Task? task = tasks.FirstOrDefault(task => task.GetStatus() == BO.Status.OnTrack);

        BO.TaskInWorker? taskInWorker = null;
        if (task != null)
            taskInWorker = new BO.TaskInWorker { Id = id, Alias = task.Alias };
        return taskInWorker;
    }
}
