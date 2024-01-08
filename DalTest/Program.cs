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
               
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

