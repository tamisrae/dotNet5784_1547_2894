using BO;
using DalApi;
using DalTest;

namespace BlTest;

partial class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();

        while (true)
        {
            Console.WriteLine("For Worker Entity press: 1");
            Console.WriteLine("For Task Entity press: 2");
            Console.WriteLine("To create a schedule press: 3");
            Console.WriteLine("To initialize data press 4");
            Console.WriteLine("For exit press: 0");

            try
            {
                if (!int.TryParse(Console.ReadLine(), out int choice))
                    throw new BlWorngValueException("WORNG VALUE");
                switch (choice)
                {
                    case 1:
                        SubMenuWorker();
                        break;
                    case 2:
                        SubMenuTask();
                        break;
                    case 3:
                        Console.WriteLine("To create an automatic schedule press: 1");
                        Console.WriteLine("To create a manual schedule press: 2");
                        if (!int.TryParse(Console.ReadLine(), out int scheduleChoice))
                            throw new BlWorngValueException("WORNG VALUE");
                        if (scheduleChoice == 1)
                            s_bl.Task.AutomaticSchedule();
                        else if (scheduleChoice == 2)
                            s_bl.Task.ManualSchedule();
                        break;
                    case 4:
                        Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                        string? answer = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                        if (answer == "Y") //stage 3
                        {
                            s_bl.Worker.Clear();
                            s_bl.Task.Clear();
                            //Initialization.Do(s_dal); //stage 2
                            Initialization.Do(); //stage 4
                        }
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid value Please try again");
                        break;
                }

            }
            catch (BlDoesNotExistsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (BlAlreadyExistsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (BlNullPropertyException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (BlWorngValueException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (BlWorkerInTaskException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (BlCantUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (BlCantDeleteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (BlTaskInWorkerException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (BlProjectStatusException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (BlScheduledDateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }


    /// <summary>
    ///  Submenu function for the worker 
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void SubMenuWorker()
    {
        Console.WriteLine("For exit press: 0");
        Console.WriteLine("To add a new worker press: 1");
        Console.WriteLine("To display worker by ID press: 2");
        Console.WriteLine("To display a list of the worker press: 3");
        Console.WriteLine("To update worker press: 4");
        Console.WriteLine("To delete worker from the list press: 5");
        Console.WriteLine("To start task press: 6");
        Console.WriteLine("To end task press: 7");
        Console.WriteLine("To sign up for a task press: 8");


        if (!int.TryParse(Console.ReadLine(), out int choice))
            throw new BlWorngValueException("WORNG VALUE");

        switch (choice)
        {
            case 1:
                CreateW();
                break;
            case 2:
                ReadW();
                break;
            case 3:
                ReadAllW();
                break;
            case 4:
                UpdateW();
                break;
            case 5:
                DeleteW();
                break;
            case 6:
                StartTask();
                break;
            case 7:
                EndTask();
                break;
            case 8:
                SignUpForTask();
                break;
            case 0:
                return;
            default:
                Console.WriteLine("Invalid value Please try again");
                break;
        }
    }

    /// <summary>
    ///  Submenu function for the task
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void SubMenuTask()
    {
        Console.WriteLine("For exit press: 0");
        Console.WriteLine("To add a new task press: 1");
        Console.WriteLine("To display task by ID press: 2");
        Console.WriteLine("To display a list of the task press: 3");   
        Console.WriteLine("To update task press: 4");
        Console.WriteLine("To delete task from the list press: 5");
        Console.WriteLine("To display the list of the task the worker can chose press: 6");
        Console.WriteLine("To display the list of the tasks by complexity press: 7");

        if (!int.TryParse(Console.ReadLine(), out int choice))
            throw new BlWorngValueException("WORNG VALUE");

        switch (choice)
        {
            case 1:
                CreateT();
                break;
            case 2:
                ReadT();
                break;
            case 3:
                ReadAllT();
                break;
            case 4:
                UpdateT();
                break;
            case 5:
                DeleteT();
                break;
            case 6:
                ListOfTasksForWorker();
                break;
            case 7:
                GroupedTasksByComplexity();
                break;
            case 0:
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid value Please try again");
                break;
        }
    }


    /// <summary>
    /// Create a new worker
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void CreateW()
    {
        Console.WriteLine("Enter ID, the worker's level, cost per hour, email and name:");

        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new BlWorngValueException("WORNG ID");
        if (!int.TryParse(Console.ReadLine(), out int level))
            throw new BlWorngValueException("WORNG LEVEL");
        if (!double.TryParse(Console.ReadLine(), out double cost))
            throw new BlWorngValueException("WORNG COST");
        string email = Console.ReadLine()!;
        string name = Console.ReadLine()!;

        BO.Worker worker = new BO.Worker { Id = id, Level = (BO.WorkerExperience)level, Email = email, Cost = cost, Name = name, CurrentTask = null };
        Console.WriteLine(s_bl.Worker.Create(worker));
        return;
    }

    /// <summary>
    /// Read worker from the data source to the logical layer
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void ReadW()
    {
        Console.WriteLine("Enter ID:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new BlWorngValueException("WORNG ID");
        Console.WriteLine(s_bl.Worker.Read(id));
        return;
    }

    /// <summary>
    /// Read all workers from the data source to the logical layer
    /// </summary>
    static void ReadAllW()
    {
        List<BO.Worker> list;
        list = s_bl.Worker.ReadAll().ToList();
        foreach (BO.Worker? worker in list)
            Console.WriteLine(worker);
    }

    /// <summary>
    /// Update worker
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void UpdateW()
    {
        Console.WriteLine("Enter ID:");
        if (!int.TryParse(Console.ReadLine(), out int workeId))
            throw new BlWorngValueException("WORNG ID");
        Console.WriteLine(s_bl.Worker.Read(workeId));

        BO.Worker worker = s_bl.Worker.Read(workeId)!;
        int id = worker.Id;
        BO.WorkerExperience level = worker.Level;
        string email = worker.Email;
        double cost = worker.Cost;
        string name = worker.Name;
        BO.TaskInWorker? currentTask = worker.CurrentTask;

        Console.WriteLine("If you want to change the level of the worker enter the new level, else press -1");
        if (!int.TryParse(Console.ReadLine(), out int newLevel))
            throw new BlWorngValueException("WORNG LEVEL");
        if (newLevel != -1)
            level = (BO.WorkerExperience)newLevel;

        Console.WriteLine("If you want to change the email enter the new email, else enter no");
        string newEmail = Console.ReadLine()!;
        if (newEmail != "no")
            email = newEmail;

        Console.WriteLine("If you want to change the cost enter the new cost, else press -1");
        if (!double.TryParse(Console.ReadLine(), out double newCost))
            throw new BlWorngValueException("WORNG COST");
        if (newCost != -1)
            cost = newCost;

        Console.WriteLine("If you want to change the name enter the new name, else enter no");
        string newName = Console.ReadLine()!;
        if (newName != "no")
            name = newName;

        BO.Worker workerToUpdate = new BO.Worker { Id = id, Level = level, Email = email, Cost = cost, Name = name, CurrentTask = currentTask };
        s_bl.Worker.Update(workerToUpdate);
    }

    /// <summary>
    /// Delete worker from the data source
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void DeleteW()
    {
        Console.WriteLine("Enter ID:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new BlWorngValueException("WORNG ID");
        s_bl.Worker.Delete(id);
    }

    /// <summary>
    /// A worker declares the start of a task
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void StartTask()
    {
        Console.WriteLine("Enter the ID of the worker:");
        if (!int.TryParse(Console.ReadLine(), out int workerId))
            throw new BlWorngValueException("WORNG ID");
        Console.WriteLine("Enter the ID of the task you want to start:");
        if (!int.TryParse(Console.ReadLine(), out int taskId))
            throw new BlWorngValueException("WORNG ID");
        BO.Task? task = s_bl.Task.Read(taskId);
        if (task != null)
            s_bl.Task.StartTask(task, workerId);
    }

    /// <summary>
    /// A worker declares the completion of a task
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void EndTask()
    {
        Console.WriteLine("Enter the ID of the worker:");
        if (!int.TryParse(Console.ReadLine(), out int workerId))
            throw new BlWorngValueException("WORNG ID");
        Console.WriteLine("Enter the ID of the task you want to finish:");
        if (!int.TryParse(Console.ReadLine(), out int taskId))
            throw new BlWorngValueException("WORNG ID");
        s_bl.Task.EndTask(taskId, workerId);
    }

    /// <summary>
    /// A worker is registered for a task
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void SignUpForTask()
    {
        Console.WriteLine("Enter the ID of the worker:");
        if (!int.TryParse(Console.ReadLine(), out int workerId))
            throw new BlWorngValueException("WORNG ID");
        Console.WriteLine("Enter the ID of the task you want to sign up to:");
        if (!int.TryParse(Console.ReadLine(), out int taskId))
            throw new BlWorngValueException("WORNG ID");
        s_bl.Task.SignUpForTask(taskId, workerId);
    }



    /// <summary>
    /// Create a new task
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void CreateT()
    {
        Console.WriteLine("Enter alias, description, complexity, deliverables");

        string alias = Console.ReadLine()!;
        string description = Console.ReadLine()!;
        DateTime createdAtDate = DateTime.Today;
        if (!int.TryParse(Console.ReadLine(), out int complexity))
            throw new BlWorngValueException("WORNG COMPLEXITY");
        string deliverables = Console.ReadLine()!;
        string remarks = Console.ReadLine()!;

        List<BO.TaskInList>? list = new();
        Console.WriteLine("If this task depends on other tasks enter 'yes' otherwise enter 'no':");
        string answer = Console.ReadLine()!;
        if (answer == "yes")
        {
            IEnumerable<BO.TaskInList> tasks = s_bl.Task.ReadAll();
            do
            {
                Console.WriteLine($"Enter the ID of the task on which the task depends:");
                try
                {
                    if (!int.TryParse(Console.ReadLine(), out int id))
                        throw new BlWorngValueException("WORNG ID");
                    BO.TaskInList? taskInList = tasks.FirstOrDefault(task => task.Id == id);
                    if (taskInList != null)
                        list.Add(taskInList);
                    else
                        throw new BlDoesNotExistsException($"Task with ID={id} doe's NOT exists");
                }
                catch (BlDoesNotExistsException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("If you want to add more dependency enter 'yes' otherwise enter 'no':");
                answer = Console.ReadLine()!;
            }
            while (answer == "yes");
        }
        if (!list.Any())
            list = null;

        BO.Task task = new BO.Task
        {
            Id = 0,
            Alias = alias,
            Description = description,
            Status = BO.Status.Unscheduled,
            WorkOnTask = null,
            Dependencies = list,
            CreatedAtDate = DateTime.Now,
            ScheduledDate = null,
            StartDate = null,
            CompleteDate = null,
            ForeCastDate = null,
            DeadlineDate = null,
            RequiredEffortTime = null,
            Deliverables = deliverables,
            Remarks = remarks,
            Complexity = (BO.WorkerExperience)complexity
        };
        Console.WriteLine(s_bl.Task.Create(task));
    }

    /// <summary>
    /// Read task from the data source to the logical layer
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void ReadT()
    {
        Console.WriteLine("Enter ID:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new BlWorngValueException("WORNG ID");
        Console.WriteLine(s_bl.Task.Read(id));
    }

    /// <summary>
    /// Read all tasks from the data source to the logical layer
    /// </summary>
    static void ReadAllT()
    {
        List<BO.TaskInList> list;
        list = s_bl.Task.ReadAll().ToList();
        foreach (TaskInList task in list)
            Console.WriteLine(task);
    }

    /// <summary>
    /// Update a task
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void UpdateT()
    {
        Console.WriteLine("Enter ID:");
        if (!int.TryParse(Console.ReadLine(), out int taskId))
            throw new BlWorngValueException("WORNG ID");
        Console.WriteLine(s_bl.Task.Read(taskId));

        BO.Task task = s_bl.Task.Read(taskId)!;

        string alias = task.Alias;
        string description = task.Description;
        BO.WorkerExperience? complexity = task.Complexity;
        TimeSpan? requiredEffortTime = task.RequiredEffortTime;
        DateTime? startDate = task.StartDate;
        DateTime? scheduledDate = task.ScheduledDate;
        DateTime? completeDate = task.CompleteDate;
        string? deliverables = task.Deliverables;
        string? remarks = task.Remarks;
        BO.WorkerInTask? workOnTask = task.WorkOnTask;


        Console.WriteLine("If you want to change the alias of the task enter the new alias, else press no");
        string newAlias = Console.ReadLine()!;
        if (newAlias != "no")
            alias = newAlias;

        Console.WriteLine("If you want to change the description of the task enter the new description, else press no");
        string newDescription = Console.ReadLine()!;
        if (newDescription != "no")
            description = newDescription;

        Console.WriteLine("If you want to change the complexity of the task enter the new complexity, else press -1");
        if (!int.TryParse(Console.ReadLine(), out int newComplexity))
            throw new BlWorngValueException("WORNG LEVEL");
        if (newComplexity != -1)
            complexity = (BO.WorkerExperience)newComplexity;

        Console.WriteLine("If you want to change the deliverables of the task enter the new deliverables, else press no");
        string newDeliverables = Console.ReadLine()!;
        if (newDeliverables != "no")
            deliverables = newDeliverables;

        Console.WriteLine("If you want to change the remarks of the task enter the new remarks, else press no");
        string newRemarks = Console.ReadLine()!;
        if (newRemarks != "no")
            remarks = newRemarks;

        Console.WriteLine("If you want to change the scheduled date enter yes else enter no");
        string scheduledChoice = Console.ReadLine()!;
        if (scheduledChoice == "yes")
        {
            Console.WriteLine("enter the new scheduled date:");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime newScheduledDate))
                throw new BlWorngValueException("WORNG DATE");
            scheduledDate = newScheduledDate;
        }

        Console.WriteLine("If you want to change the required effort time enter yes else enter no");
        string choiseRequiredEffortTime = Console.ReadLine()!;
        if (choiseRequiredEffortTime == "yes")
        {
            Console.WriteLine("enter the new required effort time:");
            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan newRequiredEffortTime))
                throw new BlWorngValueException("WORNG DATE");
            requiredEffortTime = newRequiredEffortTime;
        }

        Console.WriteLine("If you want to change the worker who work on this task enter yes else enter no");
        string answer = Console.ReadLine()!;
        if (answer == "yes")
        {
            Console.WriteLine("Enter the ID of the worker:");
            if (!int.TryParse(Console.ReadLine(), out int workerId))
                Console.WriteLine("Enter Name of the worker:");
            string name = Console.ReadLine()!;
            workOnTask = new BO.WorkerInTask { Id = workerId, Name = name };
        }

        List<BO.TaskInList> dependencies = new List<BO.TaskInList>();
        Console.WriteLine("If you want to change the dependencies of this task enter yes else enter no");
        answer = Console.ReadLine()!;
        if (answer == "yes")
        {
            IEnumerable<BO.TaskInList> tasks = s_bl.Task.ReadAll();
            do
            {
                Console.WriteLine($"Enter the ID of the task on which the task depends:");
                try
                {
                    if (!int.TryParse(Console.ReadLine(), out int id))
                        throw new BlWorngValueException("WORNG ID");
                    BO.TaskInList? taskInList = tasks.FirstOrDefault(task => task.Id == id);
                    if (taskInList != null)
                        dependencies.Add(taskInList);
                    else
                        throw new BlDoesNotExistsException($"Task with ID={id} doe's NOT exists");
                }
                catch (BlDoesNotExistsException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("If you want to add more dependency enter 'yes' otherwise enter 'no':");
                answer = Console.ReadLine()!;
            }
            while (answer == "yes");
        }

        BO.Task taskToUpdate = new BO.Task
        {
            Id = task.Id,
            Alias = alias,
            Description = description,
            Status = task.Status,
            WorkOnTask = workOnTask,
            Dependencies = dependencies,
            CreatedAtDate = task.CreatedAtDate,
            ScheduledDate = scheduledDate,
            StartDate = task.StartDate,
            CompleteDate = task.CompleteDate,
            ForeCastDate = task.ForeCastDate,
            DeadlineDate = null,
            RequiredEffortTime = requiredEffortTime,
            Deliverables = deliverables,
            Remarks = remarks,
            Complexity = complexity
        };

        s_bl.Task.Update(taskToUpdate);
    }

    /// <summary>
    /// Delete a task from the data source
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void DeleteT()
    {
        Console.WriteLine("Enter ID:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new BlWorngValueException("WORNG ID");
        s_bl.Task.Delete(id);
    }

    /// <summary>
    /// Returns all the tasks that worker can take
    /// </summary>
    /// <exception cref="BlWorngValueException"></exception>
    static void ListOfTasksForWorker()
    {
        Console.WriteLine("Enter  worker Id:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new BlWorngValueException("WORNG ID");
        IEnumerable<BO.TaskInList>? tasksForWorker;
        tasksForWorker = s_bl.Task.TasksForWorker(id);
        if (tasksForWorker != null)
        {
            foreach (TaskInList task in tasksForWorker)
                Console.WriteLine(task);
        }
    }

    /// <summary>
    /// Group tasks by complexity
    /// </summary>
    static void GroupedTasksByComplexity()
    {
        var groupedTasks = s_bl.Task.GroupTasksByComplexity();

        foreach (var group in groupedTasks)
        {
            Console.WriteLine($"Complexity Group: {group.Key ?? -1}");

            foreach (var task in group)
            {
                Console.WriteLine($"  Task ID: {task.Id}");
                Console.WriteLine($"  Alias: {task.Alias}");
                Console.WriteLine($"  Description: {task.Description}");
                Console.WriteLine($"  Status: {task.Status}");
                Console.WriteLine();
            }
        }
    }

}
