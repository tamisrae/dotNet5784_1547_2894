
namespace DalTest;
using DalApi;
using DO;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Xml.Linq;

public static class Initialization
{
    private static IWorker? s_dalWorker; //stage 1
    private static ITask? s_dalTask; //stage 1
    private static IDependency? s_dalDependency; //stage 1

    private static readonly Random s_rand = new();

    private static void createWorker()
    {
        string[] WorkerNames =
        {
            "Eyal Shani", "Nathan Weiss", "Yossi Chaimson", "Yanon Bikel", "Benny Choen",
            "Ilana Shalom", "Danny Amar", "Shimrit Nechama", "Shlomo Mendel"
        };
        string[] WorkerEmails =
        {
            "eyalShani@gmail.com", "nathanWeiss@gmail.com", "yossiChaimson@gmail.com",
            "yanoBikel@gmail.com", "bennyChoen@gmail.com", "ilanaShalom@gmail.com",
             "dannyAmar@gmail.com", "shimritNechama@gmail.com", "shlomoMendel@gmail.com"
        };

        foreach (string name in WorkerNames)
        {
            int workerId, MinWorkerId = 200000000, MaxWorkerId = 400000000;
            do
                workerId = s_rand.Next(MinWorkerId, MaxWorkerId);
            while (s_dalWorker!.Read(workerId) != null);

            int i = 0;
            while (name != WorkerNames[i])
                i++;
            string email = WorkerEmails[i];

            DO.WorkerExperience level = 0;
            if (i == 0)
                level = 0;
            else if (i == 1)
                level = (DO.WorkerExperience)1;
            else if (i == 2 || i == 3 || i == 4)
                level = (DO.WorkerExperience)2;
            else if(i == 5)
                level = (DO.WorkerExperience)3;
            else if (i==6)
                level = (DO.WorkerExperience)4;
            else if (i == 7)
                level = (DO.WorkerExperience)5;
            else if (i == 8)
                level = (DO.WorkerExperience)6;

            double cost = 0.0;
            if (i == 0)
                cost = 200;
            else if (i == 1)
                cost = 140;
            else if (i == 2 || i == 3 || i == 4)
                cost = 30;
            else if (i == 5)
                cost = 35;
            else if (i == 6)
                cost = 50;
            else if (i == 7)
                cost = 45 ;
            else if (i == 8)
                cost = 175;

            Worker newWorker = new(workerId, level, email, cost, name);
            s_dalWorker!.Create(newWorker);
        }
    }
   

    
    private static void createTask()
    {

    }
    private static void createDependency()
    {

    }
}
