using Dal;
using DalApi;
using DO;
using System.Reflection.Emit;

namespace DalTest
{
    partial class Program
    {
        private static IWorker? s_dalWorker = new WorkerImplementation(); //stage 1
        private static ITask? s_dalTask = new TaskImplementation(); //stage 1
        private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalWorker, s_dalTask, s_dalDependency);

                Console.WriteLine("For Worker Entity press: w");
                Console.WriteLine("For Task Entity press: t");
                Console.WriteLine("For Dependency Entity press: d");
                Console.WriteLine("For exit press: e");
                char choise = (char)System.Console.Read();

                string str;

                while (choise != 'e')
                { 
                    switch (choise)
                    {
                        case 'w':
                            str = "worker";
                            SubMenuWorker(str);
                            break;
                        case 't':
                            str = "task";
                            SubMenuTask(str);
                            break;
                        case 'd':
                            str = "dependency";
                            SubMenuDependency(str);
                            break;
                        default:
                            Console.WriteLine("Invalid value Please try again");
                            break;
                    }
                    if(choise =='e')
                        Environment.Exit(0);
                }
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.Message);
            }
        }



        static void SubMenuWorker(string str)
        {
            Console.WriteLine("For exit press: e");
            Console.WriteLine("To add a new {0} press: c", str);
            Console.WriteLine("To display {0} by ID press: r", str);
            Console.WriteLine("To display a list of the {0} press: c", str + 's');
            Console.WriteLine("To update {0} press: r", str);
            Console.WriteLine("To delete {0} from the list press: r", str);

            char choise = (char)System.Console.Read();

            switch (choise)
            {

            }

        }

        static void SubMenuTask(string str)
        {
            Console.WriteLine("For exit press: e");
            Console.WriteLine("To add a new {0} press: c", str);
            Console.WriteLine("To display {0} by ID press: r", str);
            Console.WriteLine("To display a list of the {0} press: c", str + 's');
            Console.WriteLine("To update {0} press: r", str);
            Console.WriteLine("To delete {0} from the list press: r", str);

            char choise = (char)System.Console.Read();
        }

        static void SubMenuDependency(string str)
        {
            string strPlural = "dependencies";

            Console.WriteLine("For exit press: e");
            Console.WriteLine("To add a new {0} press: c", str);
            Console.WriteLine("To display {0} by ID press: r", str);
            Console.WriteLine("To display a list of the {0} press: c", strPlural);
            Console.WriteLine("To update {0} press: r", str);
            Console.WriteLine("To delete {0} from the list press: r", str);

            char choise = (char)System.Console.Read();
        }

        static void CreateW()
        {
            Console.WriteLine("Enter ID, the worker's level, cost per hour, email and name:");

            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("WORNG ID");
            if (!int.TryParse(Console.ReadLine(), out int level))
                throw new Exception("WORNG LEVEL");
            string email = Console.ReadLine()!;
            if (!double.TryParse(Console.ReadLine(), out double cost))
                throw new Exception("WORNG COST");
            string name = Console.ReadLine()!;

            DO.Worker worker = new DO.Worker(id, (DO.WorkerExperience)level, email, cost, name);
            Console.WriteLine(s_dalWorker!.Create(worker));
        }

        static void ReadW()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("WORNG ID");
            Console.WriteLine(s_dalWorker!.Read(id));
        }

        static void ReadAllW()
        {
            List<Worker> list;
            list = s_dalWorker!.ReadAll();
            foreach (Worker worker in list)
                Console.WriteLine(worker);
        }

        static void Updatew()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int workeId))
                throw new Exception("WORNG ID");
            Console.WriteLine(s_dalWorker!.Read(workeId));

            Worker worker = s_dalWorker!.Read(workeId)!;
            int id = worker.Id;
            DO.WorkerExperience level = worker.Level;
            string email = worker.Email;
            double cost = worker.Cost;
            string name = worker.Name;

            Console.WriteLine("If you want to change the ID enter the new ID, else press -1");
            if (!int.TryParse(Console.ReadLine(), out int newId))
                throw new Exception("WORNG ID");
            if (newId != -1)
                id = newId;

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
            s_dalWorker!.Update(worker1);
        }

        static void DeleteW()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("WORNG ID");
            s_dalWorker!.Delete(id);
        }



        static void CreateT()
        {
            Console.WriteLine("Enter alias, description, created at date, is mile stone, id, complexity, worker id, deliverables, remarks");

            string alias = Console.ReadLine();
            string description = Console.ReadLine();
            DateTime createdAtDate = DateTime.Today;
            bool isMilestone = false;
            int id = 0;
            if (!int.TryParse(Console.ReadLine(), out int complexity))
                throw new Exception("WORNG COMPLEXITY");
            int workerId = 0;
            string deliverables = Console.ReadLine();
            string remarks = Console.ReadLine();

            DO.Task task = new DO.Task(alias, description, createdAtDate, isMilestone, id, complexity, workerId, null, null, null, null, null, deliverables, remarks);
            Console.WriteLine(s_dalTask!.Create(task));
        }

        static void ReadT()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("WORNG ID");
            Console.WriteLine(s_dalTask!.Read(id));
        }

        static void ReadAllT()
        {
            List<DO.Task> list;
            list = s_dalTask!.ReadAll();
            foreach (DO.Task task in list)
                Console.WriteLine(task);
        }

        static void UpdateT()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int taskId))
                throw new Exception("WORNG ID");
            Console.WriteLine(s_dalTask!.Read(taskId));

            DO.Task task = s_dalTask!.Read(taskId)!;

            string alias = task.Alias;
            string description = task.Description;
            //DateTime CreatedAtDate = task.CreatedAtDate;//ask!
            bool isMilestone = task.IsMilestone;
            DO.WorkerExperience? complexity = task.Complexity;
            int? workerId = task.WorkerId;
            TimeSpan? requiredEffortTime = task.RequiredEffortTime;//ask!
            DateTime? startDate = task.StartDate;//ask!
            DateTime? scheduledDate = task.ScheduledDate;//ask!
            DateTime? deadlinedate = task.Deadlinedate;//ask!
            DateTime? completeDate = task.CompleteDate;//ask!
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

            DO.Task task1 = new DO.Task(alias, description, task.CreatedAtDate, isMilestone, 0, (DO.WorkerExperience)complexity!, workerId, requiredEffortTime, startDate,
                                       scheduledDate, deadlinedate, completeDate, deliverables, remarks);

            s_dalTask!.Update(task1);
        }

        static void DeleteT()
        {
            Console.WriteLine("Enter ID:");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("WORNG ID");
            s_dalTask!.Delete(id);
        }
    }
}

