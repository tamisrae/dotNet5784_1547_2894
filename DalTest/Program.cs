﻿

namespace DalTest
{
    using Dal;
    using DalApi;
    using DO;
    using DalList;
    partial class Program
    {
        private static IDal s_dal = new Dal.DalList(); //stage 2

        //private static IWorker? s_dalWorker = new WorkerImplementation(); //stage 1
        //private static ITask? s_dalTask = new TaskImplementation(); //stage 1
        //private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dal);

                while (true)
                {

                    Console.WriteLine("For Worker Entity press: 1");
                    Console.WriteLine("For Task Entity press: 2");
                    Console.WriteLine("For Dependency Entity press: 3");
                    Console.WriteLine("For exit press: 0");

                    if (!int.TryParse(Console.ReadLine(), out int choice))
                        throw new Exception("WORNG NUMBER");
                    switch (choice)
                    {
                        case 1:
                            SubMenuWorker();
                            break;
                        case 2:
                            SubMenuTask();
                            break;
                        case 3:
                            SubMenuDependency();
                            break;
                        case 0:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid value Please try again");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        static void SubMenuWorker()
        {
            Console.WriteLine("For exit press: 0");
            Console.WriteLine("To add a new worker press: 1");
            Console.WriteLine("To display worker by ID press: 2");
            Console.WriteLine("To display a list of the worker press: 3");
            Console.WriteLine("To update worker press: 4");
            Console.WriteLine("To delete worker from the list press: 5");

            if (!int.TryParse(Console.ReadLine(), out int choice))
                throw new Exception("WORNG NUMBER");

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
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid value Please try again");
                    break;
            }
        }

        static void SubMenuTask()
        {
            Console.WriteLine("For exit press: 0");
            Console.WriteLine("To add a new task press: 1");
            Console.WriteLine("To display task by ID press: 2");
            Console.WriteLine("To display a list of the task press: 3");
            Console.WriteLine("To update task press: 4");
            Console.WriteLine("To delete task from the list press: 5");

            if (!int.TryParse(Console.ReadLine(), out int choice))
                throw new Exception("WORNG NUMBER");

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
                case 0:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid value Please try again");
                    break;
            }
        }

        static void SubMenuDependency()
        {
            Console.WriteLine("For exit press: 0");
            Console.WriteLine("To add a new dependency press: 1");
            Console.WriteLine("To display dependency by ID press: 2");
            Console.WriteLine("To display a list of the dependency press: 3");
            Console.WriteLine("To update dependency press: 4");
            Console.WriteLine("To delete dependency from the list press: 5");

            if (!int.TryParse(Console.ReadLine(), out int choice))
                throw new Exception("WORNG NUMBER");

            switch (choice)
            {
                case 1:
                    CreateD();
                    break;
                case 2:
                    ReadD();
                    break;
                case 3:
                    ReadAllD();
                    break;
                case 4:
                    UpdateD();
                    break;
                case 5:
                    DeleteD();
                    break;
                case 0:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid value Please try again");
                    break;
            }
            return;
        }



        static void CreateW()
        {
            Console.WriteLine("Enter ID, the worker's level, cost per hour, email and name:");

            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("WORNG ID");
            if (!int.TryParse(Console.ReadLine(), out int level))
                throw new Exception("WORNG LEVEL");
            if (!double.TryParse(Console.ReadLine(), out double cost))
                throw new Exception("WORNG COST");
            string email = Console.ReadLine()!;
            string name = Console.ReadLine()!;

            DO.Worker worker = new DO.Worker(id, (DO.WorkerExperience)level, email, cost, name);
            Console.WriteLine(s_dal!.Worker.Create(worker));
            return;
        }

        static void ReadW()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("WORNG ID");
            Console.WriteLine(s_dal!.Worker.Read(id));
            return;
        }

        static void ReadAllW()
        {
            List<Worker?> list;
            list = s_dal!.Worker.ReadAll().ToList();
            foreach (Worker? worker in list)
                Console.WriteLine(worker);
        }

        static void UpdateW()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int workeId))
                throw new Exception("WORNG ID");
            Console.WriteLine(s_dal!.Worker.Read(workeId));

            Worker worker = s_dal!.Worker.Read(workeId)!;
            int id = worker.Id;
            DO.WorkerExperience level = worker.Level;
            string email = worker.Email;
            double cost = worker.Cost;
            string name = worker.Name;

            Console.WriteLine("If you want to change the level of the worker enter the new level, else press -1");
            if (!int.TryParse(Console.ReadLine(), out int newLevel))
                throw new Exception("WORNG LEVEL");
            if (newLevel != -1)
                level = (DO.WorkerExperience)newLevel;

            Console.WriteLine("If you want to change the email enter the new email, else press no");
            string newEmail = Console.ReadLine()!;
            if(newEmail != "no")
                email = newEmail;

            Console.WriteLine("If you want to change the cost enter the new cost, else press -1");
            if (!double.TryParse(Console.ReadLine(), out double newCost))
                throw new Exception("WORNG COST");
            if (newCost != -1)
                cost = newCost;

            Console.WriteLine("If you want to change the name enter the new name, else press no");
            string newName = Console.ReadLine()!;
            if (newName != "no")
                name = newName;

            Worker worker1 = new Worker(id, level, email, cost, name);
            s_dal!.Worker.Update(worker1);
        }

        static void DeleteW()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("WORNG ID");
            s_dal!.Worker.Delete(id);
        }



        static void CreateT()
        {
            Console.WriteLine("Enter alias, description, complexity, deliverables, remarks");

            string alias = Console.ReadLine()!;
            string description = Console.ReadLine()!;
            DateTime createdAtDate = DateTime.Today;
            bool isMilestone = false;
            int id = 0;
            if (!int.TryParse(Console.ReadLine(), out int complexity))
                throw new Exception("WORNG COMPLEXITY");
            int workerId = 0;
            string deliverables = Console.ReadLine()!;
            string remarks = Console.ReadLine()!;

            DO.Task task = new DO.Task(alias, description, createdAtDate, isMilestone, id, (DO.WorkerExperience)complexity, workerId, null, null, null, null, null, deliverables, remarks);
            Console.WriteLine(s_dal!.Task.Create(task));
        }

        static void ReadT()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("WORNG ID");
            Console.WriteLine(s_dal!.Task.Read(id));
        }

        static void ReadAllT()
        {
            List<Task?> list;
            list = s_dal!.Task.ReadAll().ToList();
            foreach (Task? task in list)
                Console.WriteLine(task);
        }

        static void UpdateT()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int taskId))
                throw new Exception("WORNG ID");
            Console.WriteLine(s_dal!.Task.Read(taskId));

            DO.Task task = s_dal!.Task.Read(taskId)!;

            string alias = task.Alias;
            string description = task.Description;
            bool isMilestone = task.IsMilestone;
            DO.WorkerExperience? complexity = task.Complexity;
            int? workerId = task.WorkerId;
            TimeSpan? requiredEffortTime = task.RequiredEffortTime;
            DateTime? startDate = task.StartDate;
            DateTime? scheduledDate = task.ScheduledDate;
            DateTime? deadlinedate = task.Deadlinedate;
            DateTime? completeDate = task.CompleteDate;
            string? deliverables = task.Deliverables;
            string? remarks = task.Remarks;


            Console.WriteLine("If you want to change the alias of the task enter the new alias, else press no");
            string newAlias = Console.ReadLine()!;
            if (newAlias != "no")
                alias = newAlias;

            Console.WriteLine("If you want to change the description of the task enter the new description, else press no");
            string newDescription = Console.ReadLine()!;
            if (newDescription != "no")
                description = newDescription;

            Console.WriteLine("If you want to change the isMilestone of the task enter the new isMilestone press 0, else press -1");
            if (!int.TryParse(Console.ReadLine(), out int newisMilestone))
                throw new Exception("WORNG isMilestone");
            if (newisMilestone == 0)
                isMilestone = !isMilestone;

            Console.WriteLine("If you want to change the complexity of the task enter the new complexity, else press -1");
            if (!int.TryParse(Console.ReadLine(), out int newComplexity))
                throw new Exception("WORNG LEVEL");
            if (newComplexity != -1)
                complexity = (DO.WorkerExperience)newComplexity;


            Console.WriteLine("If you want to change the worker ID of the task enter the new worker ID, else press -1");
            if (!int.TryParse(Console.ReadLine(), out int newWorkerId))
                throw new Exception("WORNG WORKER ID");
            if (newComplexity != -1)
                workerId = newWorkerId;

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
                    throw new Exception("WORNG DATE");
                scheduledDate = newScheduledDate;
            }

            Console.WriteLine("If you want to change the required effort time enter yes else enter no");
            string choiseRequiredEffortTime = Console.ReadLine()!;
            if (choiseRequiredEffortTime == "yes")
            {
                Console.WriteLine("enter the new required effort time:");
                if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan newRequiredEffortTime))
                    throw new Exception("WORNG DATE");
                requiredEffortTime = newRequiredEffortTime;
            }

            Console.WriteLine("If you want to change the dead line date enter yes else enter no");
            string deadlinedateChoise = Console.ReadLine()!;
            if (deadlinedateChoise == "yes")
            {
                Console.WriteLine("enter the new dead line date date:");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime newDeadlinedate))
                    throw new Exception("WORNG DATE");
                deadlinedate = newDeadlinedate;
            }

            Task task1 = new Task(alias, description, task.CreatedAtDate, isMilestone, task.Id, (DO.WorkerExperience)complexity!, workerId, requiredEffortTime, startDate,
                                       scheduledDate, deadlinedate, completeDate, deliverables, remarks);

            s_dal!.Task.Update(task1);
        }

        static void DeleteT()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("WORNG ID");
            s_dal!.Task.Delete(id);
        }



        static void CreateD()
        {
            Console.WriteLine("Enter The dependent task and depends on task:");

            if (!int.TryParse(Console.ReadLine(), out int dependentTask))
                throw new Exception("WORNG ID");
            if (!int.TryParse(Console.ReadLine(), out int dependsOnTask))
                throw new Exception("WORNG ID");

            Dependency dependency = new Dependency(0, dependentTask, dependsOnTask);
            Console.WriteLine(s_dal!.Dependency.Create(dependency));
        }

        static void ReadD()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("WORNG ID");
            Console.WriteLine(s_dal!.Dependency.Read(id));
        }

        static void ReadAllD()
        {
            List<Dependency?> list;
            list = s_dal!.Dependency.ReadAll().ToList();
            foreach (Dependency? dependency in list)
                Console.WriteLine(dependency);
        }

        static void UpdateD()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int dependencyId))
                throw new Exception("WORNG ID");
            Console.WriteLine(s_dal!.Dependency.Read(dependencyId));

            Dependency dependency= s_dal!.Dependency.Read(dependencyId)!;
            int dependentTask = dependency.DependentTask;
            int dependsOnTask = dependency.DependsOnTask;

            Console.WriteLine("If you want to change the dependent task enter the new dependent task, else press -1");
            if (!int.TryParse(Console.ReadLine(), out int newDependentTask))
                throw new Exception("WORNG ID");
            if (newDependentTask != -1)
                dependentTask = newDependentTask;

            Console.WriteLine("If you want to change the depends on task enter the new dependent task, else press -1");
            if (!int.TryParse(Console.ReadLine(), out int newDependsOnTask))
                throw new Exception("WORNG ID");
            if (newDependsOnTask != -1)
                dependsOnTask = newDependsOnTask;

            Dependency dependency1 = new Dependency (dependency.Id, dependentTask, dependsOnTask);
            s_dal!.Dependency.Update(dependency1);
        }

        static void DeleteD()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("WORNG ID");
            s_dal!.Dependency.Delete(id);
        }
    }
}

