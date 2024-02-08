using BlApi;
using BO;
using DO;
using System.Data;

namespace BlImplementation;
internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal dal = DalApi.Factory.Get;

    public int Create(BO.Task task)
    {
        BO.ProjectStatus projectStatus = IBl.GetProjectStatus();
        if (projectStatus != BO.ProjectStatus.Unscheduled)
            throw new BlProjectStatusException("You cannot create new task in this stage at the project");
        else
        {
            if (task.Dependencies != null)
            {
                IEnumerable<int> dependencies = from BO.TaskInList item in task.Dependencies
                                                let dependency = new DO.Dependency(0, task.Id, item.Id)
                                                select dal.Dependency.Create(dependency);
            }

            DO.Task doTask = DataChecking(task);
            try
            {
                int idTask = dal.Task.Create(doTask);
                return idTask;
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlAlreadyExistsException($"Task with ID={task.Id} already exists", ex);
            }
        }
    }

    public void Delete(int id)
    {
        BO.ProjectStatus projectStatus = IBl.GetProjectStatus();
        if (projectStatus != BO.ProjectStatus.Unscheduled)
            throw new BlProjectStatusException("Yoe cannot delete the task in this stage at the project");
        else
        {
            try
            {
                DO.Task? task = dal.Task.Read(id);
                if (task == null)
                    throw new BO.BlDoesNotExistsException($"Task with ID={id} doe's NOT exists");

                IEnumerable<DO.Dependency>? dependencies = from DO.Dependency item in dal.Dependency.ReadAll()
                                                           let taskId = item.DependsOnTask
                                                           where taskId == id
                                                           select item;
                if (dependencies != null)
                    throw new BlCantDeleteException("This Task Cannot be deleted");

                dal.Task.Delete(id);
            }
            catch (DO.DalDoesNotExistsException ex)
            {
                throw new BO.BlDoesNotExistsException($"Task with ID={id} doe's NOT exists", ex);
            }
        }
    }

    public BO.Task? Read(int id)
    {
        try
        {
            DO.Task? doTask = dal.Task.Read(id);

            if (doTask == null)
                throw new BO.BlDoesNotExistsException($"Task with ID={id} doe's NOT exists");
            else
            {
                BO.WorkerInTask? workerInTask;
                if (doTask.WorkerId != null)
                {
                    DO.Worker worker = dal.Worker.Read((int)doTask.WorkerId)!;
                    workerInTask = new BO.WorkerInTask { Id = (int)doTask.WorkerId, Name = worker.Name };
                }
                else
                    workerInTask = null;

                int? temp = null;
                BO.WorkerExperience? complexity = null;
                if (doTask.Complexity != null)
                {
                    temp = (int)doTask.Complexity;
                    complexity = (BO.WorkerExperience)temp;
                }

                IEnumerable<DO.Dependency>? dependencies = FindDependencies(id);
                List<TaskInList>? taskInLists = null;
                if (dependencies != null)
                {
                    taskInLists = (from DO.Dependency dependency in dependencies
                                   select ReadAll().FirstOrDefault(task => task.Id == dependency.DependsOnTask)).ToList();
                }

                return new BO.Task
                {
                    Id = id,
                    Alias = doTask.Alias,
                    Description = doTask.Description,
                    Status = doTask.GetStatus(),
                    WorkOnTask = workerInTask,
                    Dependencies = taskInLists,
                    CreatedAtDate = doTask.CreatedAtDate,
                    ScheduledDate = doTask.ScheduledDate,
                    StartDate = doTask.StartDate,
                    CompleteDate = doTask.CompleteDate,
                    ForeCastDate = doTask.GetForeCastDate(),
                    DeadlineDate = doTask.Deadlinedate,
                    RequiredEffortTime = doTask.RequiredEffortTime,
                    Deliverables = doTask.Deliverables,
                    Remarks = doTask.Remarks,
                    Complexity = complexity
                };
            }
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Task with ID={id} doe's NOT exists", ex);
        }
    }

    public IEnumerable<TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if (filter == null)
        {
            return from item in dal.Task.ReadAll()
                   select new BO.TaskInList
                   {
                       Id = item.Id,
                       Alias = item.Alias,
                       Description = item.Description,
                       Status = item.GetStatus()
                   };
        }
        else
        {
            return from item in dal.Task.ReadAll()
                   where filter!(Read(item.Id)!)
                   select new BO.TaskInList
                   {
                       Id = item.Id,
                       Alias = item.Alias,
                       Description = item.Description,
                       Status = item.GetStatus()
                   };
        }
    }

    public IEnumerable<TaskInList>? TasksForWorker(int workerId)
    {
        try
        {
            DO.Worker? worker = dal.Worker.Read(workerId);
            if (worker != null)
            {
                IEnumerable<BO.TaskInList>? tasks = from task in dal.Task.ReadAll()
                                                    where task.WorkerId == null && task.Complexity == worker.Level
                                                    select new BO.TaskInList
                                                    {
                                                        Id = task.Id,
                                                        Alias = task.Alias,
                                                        Description = task.Description,
                                                        Status = task.GetStatus()
                                                    };
                return tasks;
            }
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Worker with ID={workerId} doe's NOT exists", ex);
        }
        return null;
    }

    public void Update(BO.Task task)
    {
        BO.ProjectStatus projectStatus = IBl.GetProjectStatus();
        if (projectStatus == BO.ProjectStatus.Scheduled || projectStatus == BO.ProjectStatus.Execution)
        {
            try
            {
                DO.Task? checkingTask = dal.Task.Read(task.Id);
                if (checkingTask != null && (task.CreatedAtDate != checkingTask.CreatedAtDate || (int?)task.Complexity != (int?)checkingTask.Complexity ||
                    task.RequiredEffortTime != checkingTask.RequiredEffortTime || task.StartDate != checkingTask.StartDate || task.ScheduledDate != checkingTask.StartDate ||
                    task.DeadlineDate != checkingTask.Deadlinedate || task.CompleteDate != checkingTask.CompleteDate))
                    throw new BlProjectStatusException("You cannot update the task in this stage at the project");
                else
                {
                    if (checkingTask != null)
                    {
                        int? workerId = null;
                        if (task.WorkOnTask != null)
                            workerId = task.WorkOnTask.Id;

                        checkingTask = checkingTask with { Alias = task.Alias, Description = task.Description, Deliverables = task.Deliverables, Remarks = task.Remarks, WorkerId = workerId };
                        dal.Task.Update(checkingTask);

                    }
                }
            }
            catch (DO.DalDoesNotExistsException ex)
            {
                throw new BO.BlDoesNotExistsException($"Task with ID={task.Id} doe's NOT exists", ex);
            }
        }

        else
        {
            if (task.Dependencies != null)
            {
                IEnumerable<DO.Dependency>? dependencies = FindDependencies(task.Id);
                if (GetDependency(task.Id, dependencies))
                {
                    IEnumerable<int> createDependencies = from BO.TaskInList task1 in task.Dependencies
                                                          let dependency = dal.Dependency.ReadAll().FirstOrDefault(dependency => dependency.DependentTask != task.Id || dependency.DependsOnTask != task1.Id)
                                                          where dependency == null
                                                          select dal.Dependency.Create(dependency);
                }
                else
                    throw new BlCantUpdateException("This task cannot be update");
            }

            DO.Task doTask = DataChecking(task);

            try
            {
                dal.Task.Update(doTask);
            }
            catch (DO.DalDoesNotExistsException ex)
            {
                throw new BO.BlDoesNotExistsException($"Task with ID={task.Id} doe's NOT exists", ex);
            }
        }
    }

    public void UpdateTheScheduledDate(int taskId, DateTime scheduledDate)
    {
        if (scheduledDate < dal.StartProjectDate)
            throw new BlScheduledDateException("You cannot enter this date for the task");

        try
        {
            BO.Task? task = Read(taskId);
            if (task != null)
            {
                if (task.WorkOnTask != null)
                {
                    IEnumerable<DO.Task>? tasksList = from DO.Task doTask in dal.Task.ReadAll()
                                                      where doTask.WorkerId == task.WorkOnTask.Id
                                                      select doTask;

                    if (tasksList.Any(doTask => (scheduledDate > doTask.ScheduledDate || scheduledDate > doTask.StartDate) && scheduledDate < doTask.GetForeCastDate()))
                    {
                        throw new BlScheduledDateException("The worker is working on another task at this time");
                    }
                }

                if (task.Dependencies != null)
                {
                    IEnumerable<BO.Task> tasks = from BO.TaskInList taskInList in task.Dependencies
                                                 select (Read(taskInList.Id));

                    if (task.Dependencies.Any(dependency => tasks.Any(t => t.StartDate == null || t.StartDate < DateTime.Now) ||
                       Read(dependency.Id)!.ScheduledDate > tasks.Max(t => t.ForeCastDate)))
                    {
                        throw new BlScheduledDateException("You cannot enter a scheduled date for this task");
                    }

                }

                DO.Task? taskToRead = dal.Task.Read(taskId);
                if (taskToRead != null)
                {
                    taskToRead = taskToRead with { ScheduledDate = scheduledDate };
                    dal.Task.Update(taskToRead);
                }

            }
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Task with ID={taskId} doe's NOT exists", ex);
        }
    }

    private IEnumerable<DO.Dependency>? FindDependencies(int id)
    {
        IEnumerable<DO.Dependency>? dependencies = (from dependency in dal.Dependency.ReadAll()
                                                    where dependency.DependentTask == id
                                                    select dependency);
        return dependencies;
    }

    private bool GetDependency(int taskId, IEnumerable<DO.Dependency>? dependencies)
    {
        if (dependencies == null)
            return true;
        else
        {

            if (dependencies.Any(dependency => dependency.DependentTask == taskId ||GetDependency(taskId, FindDependencies(dependency.Id)) == false))
            {
                return false;
            }
            return true;
        }
    }

    private DO.Task DataChecking(BO.Task task)
    {
        if (task.Id.IsGreaterThanZero() || task.Alias.IsEmptyString())
            throw new BlWorngValueException("The task has WORNG VALUE!");

        int? temp = (int?)task.Complexity;
        DO.WorkerExperience? complexity = null;
        if (temp != null)
            complexity = (DO.WorkerExperience)temp;

        if (task.ScheduledDate != null)
        {
            BO.ProjectStatus projectStatus = IBl.GetProjectStatus();
            if (projectStatus == BO.ProjectStatus.Unscheduled)
                throw new BlProjectStatusException("You cannot enter a scheduled start date for a task at this stage of the project");
            else
                UpdateTheScheduledDate(task.Id, (DateTime)task.ScheduledDate);
        }

        int? workerId = null;
        if (task.WorkOnTask != null)
            workerId = task.WorkOnTask.Id;

        DO.Task doTask = new DO.Task(task.Alias, task.Description, task.CreatedAtDate, false, task.Id, complexity, task.WorkOnTask?.Id, task.RequiredEffortTime,
        task.StartDate, task.ScheduledDate, task.DeadlineDate, task.CompleteDate, task.Deliverables, task.Remarks);

        return doTask;
    }

    public void ManualSchedule()
    {
        dal.StartProjectDate = DateTime.Now;
        foreach (DO.Task task in dal.Task.ReadAll())
        {
            Console.WriteLine("enter the scheduled date:");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime scheduledDate))
                throw new DalWorngValueException("WORNG DATE");
            DO.Task tempTask = task with { ScheduledDate = scheduledDate };
            UpdateTheScheduledDate(task.Id, scheduledDate);
            try
            {
                dal.Task.Update(tempTask);
            }
            catch (DO.DalDoesNotExistsException ex)
            {
                throw new BO.BlDoesNotExistsException($"Task with ID={task.Id} doe's NOT exists", ex);
            }
        }
    }

    public void AutomaticSchedule()
    {
        dal.StartProjectDate = DateTime.Now;
        foreach (DO.Task task in dal.Task.ReadAll())
        {
            BO.Task? boTask = Read(task.Id);
            DateTime? dateTime;
            if (boTask != null)
            {
                dateTime = IBl.ScheduleDateOffer(boTask);
                DO.Task taskToUpdate = task with { ScheduledDate = dateTime };
                try
                {
                    dal.Task.Update(taskToUpdate);
                }
                catch (DO.DalDoesNotExistsException ex)
                {
                    throw new BO.BlDoesNotExistsException($"Task with ID={task.Id} doe's NOT exists", ex);
                }
            }
        }
    }
}