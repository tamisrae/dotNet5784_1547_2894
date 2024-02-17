using BlApi;
using BO;
using DO;
using System.Data;

namespace BlImplementation;
internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal dal = DalApi.Factory.Get;

    /// <summary>
    /// This function create a new logic task
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    /// <exception cref="BlProjectStatusException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
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

    /// <summary>
    /// This function delete a logic task
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BlProjectStatusException"></exception>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    /// <exception cref="BlCantDeleteException"></exception>
    public void Delete(int id)
    {
        BO.ProjectStatus projectStatus = IBl.GetProjectStatus();
        if (projectStatus != BO.ProjectStatus.Unscheduled)
            throw new BlProjectStatusException($"You cannot delete task with ID={id} in this stage at the project");
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
                    throw new BlCantDeleteException($"Task with ID={id} Cannot be deleted");

                dal.Task.Delete(id);
            }
            catch (DO.DalDoesNotExistsException ex)
            {
                throw new BO.BlDoesNotExistsException($"Task with ID={id} doe's NOT exists", ex);
            }
        }
    }

    /// <summary>
    /// This function read logic task
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
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

    /// <summary>
    /// This function returns all the tasks from the data source
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if (filter == null)//without filter
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
        else//with filter
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

    /// <summary>
    /// This function returns list of tasks that worker can take 
    /// </summary>
    /// <param name="workerId"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
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

    /// <summary>
    /// This function update logic task
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BlProjectStatusException"></exception>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    /// <exception cref="BlCantUpdateException"></exception>
    public void Update(BO.Task task)
    {
        BO.ProjectStatus projectStatus = IBl.GetProjectStatus();
        if (projectStatus == BO.ProjectStatus.Execution)
        {
            try
            {
                //If the user has changed fields that should not be changed at this stage of the project
                DO.Task? checkingTask = dal.Task.Read(task.Id);
                if (checkingTask != null && (task.CreatedAtDate != checkingTask.CreatedAtDate || (int?)task.Complexity != (int?)checkingTask.Complexity ||
                    task.RequiredEffortTime != checkingTask.RequiredEffortTime || task.StartDate != checkingTask.StartDate || task.ScheduledDate != checkingTask.StartDate ||
                    task.DeadlineDate != checkingTask.Deadlinedate || task.CompleteDate != checkingTask.CompleteDate))
                    throw new BlProjectStatusException($"You cannot update task with ID={task.Id} in this stage at the project");
                else if (task.Dependencies != null)
                {
                    List<BO.TaskInList> dependencies = (from BO.TaskInList taskInList in task.Dependencies
                                                        where dal.Dependency.ReadAll().FirstOrDefault(dependency => dependency.DependentTask == task.Id && dependency.DependsOnTask == taskInList.Id) == null
                                                        select taskInList).ToList();
                    if (dependencies.Any())
                        throw new BlProjectStatusException($"You cannot update task with ID={task.Id} in this stage at the project");
                }
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
                    throw new BlCantUpdateException($"Task with ID={task.Id} cannot be update");
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

    /// <summary>
    /// This function update the scheduled date of a task
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="scheduledDate"></param>
    /// <exception cref="BlScheduledDateException"></exception>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    public void UpdateTheScheduledDate(int taskId, DateTime scheduledDate)
    {
        if (scheduledDate < dal.StartProjectDate)
            throw new BlScheduledDateException($"You cannot enter this date for task with ID={taskId}");
        else
        {
            try
            {
                BO.Task? task = Read(taskId);
                if (task != null)
                {
                    //Check that the worker is not working on another task at this time
                    if (task.WorkOnTask != null)
                    {
                        IEnumerable<DO.Task>? tasksList = from DO.Task doTask in dal.Task.ReadAll()
                                                          where doTask.WorkerId == task.WorkOnTask.Id
                                                          select doTask;

                        if (tasksList.Any(doTask => (scheduledDate > doTask.ScheduledDate || scheduledDate > doTask.StartDate) && scheduledDate < doTask.GetForeCastDate()))
                        {
                            throw new BlScheduledDateException($"Worker with ID={task.WorkOnTask.Id} is working on another task at this time");
                        }
                    }

                    if (task.Dependencies!.Count != 0)
                    {
                        List<BO.Task> tasks = (from BO.TaskInList taskInList in task.Dependencies
                                               select (Read(taskInList.Id))).ToList();

                        if (task.Dependencies.Any(dependency => tasks.Any(t => t.ScheduledDate == null ||
                        Read(dependency.Id)!.ScheduledDate > tasks.Max(t => t.ForeCastDate))))
                        {
                            throw new BlScheduledDateException($"You cannot enter a scheduled date for task with ID={taskId}");
                        }

                    }

                    DO.Task? taskToUpdate = dal.Task.Read(taskId);
                    if (taskToUpdate != null)
                    {
                        taskToUpdate = taskToUpdate with { ScheduledDate = scheduledDate };
                        dal.Task.Update(taskToUpdate);
                    }

                }
            }
            catch (DO.DalDoesNotExistsException ex)
            {
                throw new BO.BlDoesNotExistsException($"Task with ID={taskId} doe's NOT exists", ex);
            }
        }
    }

    /// <summary>
    /// This function returns all the dependencies of a task
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private List<DO.Dependency>? FindDependencies(int id)
    {
        List<DO.Dependency>? dependencies = (from dependency in dal.Dependency.ReadAll()
                                             where dependency.DependentTask == id
                                             select dependency).ToList();
        return dependencies;
    }

    /// <summary>
    /// This function checks if there is a circular dependency
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="dependencies"></param>
    /// <returns></returns>
    private bool GetDependency(int taskId, IEnumerable<DO.Dependency>? dependencies)
    {
        if (dependencies == null)
            return true;
        else
        {
            if (dependencies.Any(dependency => dependency.DependentTask == taskId || GetDependency(taskId, FindDependencies(dependency.Id)) == false))
                return false;
            return true;
        }
    }

    /// <summary>
    /// This function does input integrity check
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    /// <exception cref="BlWorngValueException"></exception>
    /// <exception cref="BlProjectStatusException"></exception>
    private DO.Task DataChecking(BO.Task task)
    {
        if (task.Id.IsGreaterThanZero() || task.Alias.IsEmptyString())
            throw new BlWorngValueException($"Task with ID={task.Id} has WORNG VALUE!");

        int? temp = (int?)task.Complexity;
        DO.WorkerExperience? complexity = null;
        if (temp != null)
            complexity = (DO.WorkerExperience)temp;

        if (task.ScheduledDate != null)
        {
            BO.ProjectStatus projectStatus = IBl.GetProjectStatus();
            if (projectStatus == BO.ProjectStatus.Unscheduled)
                throw new BlProjectStatusException($"You cannot enter a scheduled start date for Task with ID={task.Id} at this stage of the project");
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

    /// <summary>
    /// This function makes a schedule manually
    /// </summary>
    /// <exception cref="DalWorngValueException"></exception>
    public void ManualSchedule()
    {
        dal.StartProjectDate = DateTime.Now;
        foreach (DO.Task task in dal.Task.ReadAll())
        {
            Console.WriteLine("enter the scheduled date:");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime scheduledDate))
                throw new DalWorngValueException("WORNG DATE");
            UpdateTheScheduledDate(task.Id, scheduledDate);
        }
    }

    /// <summary>
    /// This function makes a schedule automatically
    /// </summary>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    public void AutomaticSchedule()
    {
        dal.StartProjectDate = DateTime.Now;
        List<BO.Task> tasks = (from DO.Task task in dal.Task.ReadAll()
                               where FindDependencies(task.Id)!.Count == 0
                               select Read(task.Id)).ToList();//All the tasks that didn't have dependencies
        foreach (BO.Task task in tasks)
        {
            try
            {
                UpdateTheScheduledDate(task.Id, (DateTime)dal.StartProjectDate);
            }
            catch (DO.DalDoesNotExistsException ex)
            {
                throw new BO.BlDoesNotExistsException($"Task with ID={task.Id} doe's NOT exists", ex);
            }
        }

        List<BO.Task> taskList = (from DO.Task task in dal.Task.ReadAll()
                                  where task.ScheduledDate == null
                                  select Read(task.Id)).ToList();
        Rec(taskList);
    }

    /// <summary>
    /// Recursive helper function to create an automatic schedule
    /// </summary>
    /// <param name="tasks"></param>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    private void Rec(List<BO.Task> tasks)
    {
        if (!tasks.Any())
            return;
        else
        {
            List<BO.Task> depentsOnTasks = new List<BO.Task>();
            List<BO.Task> tasksToRemove = new List<BO.Task>();
            foreach (BO.Task task in tasks)
            {
                if (task.Dependencies != null)
                {
                    depentsOnTasks = (from BO.TaskInList taskInList in task.Dependencies
                                      select Read(taskInList.Id)).ToList();

                    if ((depentsOnTasks.FirstOrDefault(t => t.ScheduledDate == null)) == null)//Check that all tasks that my task depends on have a scheduled date
                    {
                        DateTime? scheduledDate = IBl.ScheduleDateOffer(task);
                        if (scheduledDate != null)
                        {
                            try
                            {
                                UpdateTheScheduledDate(task.Id, (DateTime)scheduledDate);
                            }
                            catch (DO.DalDoesNotExistsException ex)
                            {
                                throw new BO.BlDoesNotExistsException($"Task with ID={task.Id} doe's NOT exists", ex);
                            }
                            tasksToRemove.Add(task);
                        }
                    }
                }
            }
            foreach (BO.Task task in tasksToRemove)
            {
                if (tasks.FirstOrDefault(t => t.Id == task.Id) != null)
                    tasks.Remove(task);
            }
            Rec(tasks);
        }
    }

    /// <summary>
    /// A worker declares the start of a task
    /// </summary>
    /// <param name="boTask"></param>
    /// <param name="workerId"></param>
    /// <exception cref="BlWorkerInTaskException"></exception>
    /// <exception cref="BlTaskInWorkerException"></exception>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    public void StartTask(BO.Task boTask, int workerId)
    {
        if (boTask.Dependencies != null)//Check that all the tasks that my task depends on have completed
        {
            List<BO.Task> tasks = (from BO.TaskInList taskInList in boTask.Dependencies
                                   select Read(taskInList.Id)).ToList();
            if (tasks.FirstOrDefault(t => t.Status != Status.Done) != null)
                throw new BlWorkerInTaskException($"You cannot work on task with ID={boTask.Id} right now");
        }

        try
        {
            DO.Task? doTask = dal.Task.Read(boTask.Id);
            //Check that the worker is registered for this task and it is in progress
            DO.Task? task = dal.Task.ReadAll().FirstOrDefault(t => t.WorkerId != null && t.WorkerId == workerId && t.GetStatus() == Status.OnTrack);

            if (task != null && task.Id != boTask.Id)
                throw new BlTaskInWorkerException($"Worker with ID={workerId} in the middle of another task");
            else if (doTask != null && doTask.WorkerId != null && doTask.WorkerId == workerId)
            {
                doTask = doTask with { StartDate = DateTime.Now };
                dal.Task.Update(doTask);
            }
            else
                throw new BlWorkerInTaskException($"Worker with ID={workerId} cannot work on task with ID={boTask.Id}");
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Task with ID={boTask.Id} doe's NOT exists", ex);
        }
    }

    /// <summary>
    /// A worker declares the completion of a task
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="workerId"></param>
    /// <exception cref="BlTaskInWorkerException"></exception>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    public void EndTask(int taskId, int workerId)
    {
        try
        {
            DO.Task? task = dal.Task.Read(taskId);
            //Check that the worker really worked on this task
            DO.Task? doTask = dal.Task.ReadAll().FirstOrDefault(t => t.WorkerId != null && t.WorkerId == workerId && t.GetStatus() == Status.OnTrack);

            if (task != null && doTask != null && doTask.Id == taskId)
            {
                task = task with { CompleteDate = DateTime.Now };
                dal.Task.Update(task);
            }
            else
                throw new BlTaskInWorkerException($"Worker with ID={workerId} did not worked on task with ID={taskId}");
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Task with ID={taskId} doe's NOT exists", ex);
        }
    }

    /// <summary>
    /// A worker is registered for a task
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="workerId"></param>
    /// <exception cref="BO.BlDoesNotExistsException"></exception>
    /// <exception cref="BlTaskInWorkerException"></exception>
    public void SignUpForTask(int taskId, int workerId)
    {
        if (TasksForWorker(workerId) != null && TasksForWorker(workerId)!.FirstOrDefault(t => t.Id == taskId) != null)
        {
            try
            {
                DO.Task? task = dal.Task.Read(taskId);
                if (task != null)
                {
                    task = task with { WorkerId = workerId };
                    dal.Task.Update(task);
                }
            }
            catch (DO.DalDoesNotExistsException ex)
            {
                throw new BO.BlDoesNotExistsException($"Task with ID={taskId} doe's NOT exists", ex);
            }
        }
        else
            throw new BlTaskInWorkerException($"Worker with ID={workerId} cannot sign up for task with ID={taskId}");
    }

    /// <summary>
    /// Group tasks by complexity
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IGrouping<int?, BO.TaskInList>> GroupTasksByComplexity()
    {
        var tasks = from task in dal.Task.ReadAll()
                    group new BO.TaskInList
                    {
                        Id = task.Id,
                        Alias = task.Alias,
                        Description = task.Description,
                        Status = task.GetStatus()
                    } by (int?)task.Complexity;
        return tasks;
    }

    /// <summary>
    /// This function clear all the data from the data layer
    /// </summary>
    public void Clear()
    {
        dal.Task.Clear();
    }
}
