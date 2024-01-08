using Dal;
using DalApi;

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

                char entityChoise = MainMenu();







            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.Message);
            }
        }


        static char MainMenu()
        {
            Console.WriteLine("For Worker Entity press: w");
            Console.WriteLine("For Task Entity press: t");
            Console.WriteLine("For Dependency Entity press: d");
            Console.WriteLine("For exit press: e");
            char choise = (char)System.Console.Read();
            return choise;
        }

        static Char SubMenuWorker(string str)
        {
            Console.WriteLine("For exit press: e");
            Console.WriteLine("To add a new {0} press: c", str);
            Console.WriteLine("To display {0} by ID press: r", str);
            Console.WriteLine("To display a list of the {0} press: c", str + 's');
            Console.WriteLine("To update {0} press: r", str);
            Console.WriteLine("To delete {0} from the list press: r", str);

            char choise = (char)System.Console.Read();


            return choise;

        }

        static Char SubMenuTask(string str)
        {
            //string str = "task";

            Console.WriteLine("For exit press: e");
            Console.WriteLine("To add a new {0} press: c", str);
            Console.WriteLine("To display {0} by ID press: r", str);
            Console.WriteLine("To display a list of the {0} press: c", str + 's');
            Console.WriteLine("To update {0} press: r", str);
            Console.WriteLine("To delete {0} from the list press: r", str);

            char choise = (char)System.Console.Read();
            return choise;

        }

        static Char SubMenuDependency(char entity)
        {
            string str = "dependency";
            string strPlural = "dependencies";

            Console.WriteLine("For exit press: e");
            Console.WriteLine("To add a new {0} press: c", str);
            Console.WriteLine("To display {0} by ID press: r", str);
            Console.WriteLine("To display a list of the {0} press: c", strPlural);
            Console.WriteLine("To update {0} press: r", str);
            Console.WriteLine("To delete {0} from the list press: r", str);

            char choise = (char)System.Console.Read();
            return choise;

        }

    }
}

