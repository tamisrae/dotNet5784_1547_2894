
namespace DalTest;
using DalApi;
using DO;
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
        string[] TaskAlias =
        {
            "Prepare the first course", "Prepare the second course", "Make the desserts", "Make the salads",//Chef's tasks 
            "Cut the vegetables", "Boil the oil", "Slice the meat", "Make a grocery list",//Sous chef's tasks
            "Serveing", "Clear the tables", "Open the bottles of wine",//Waiter's tasks
            "Wash the dishes", "Wash the floor", "Wipe the dishes", "Put the dishes in place",//Cleaner's tasks
            "Drives the food", "Returns the dishes", "Load the food",//Driver's tasks
            "Accepting the order", "Collects the payment", "Take care of a shopping list",//Secretary's tasks
            "Take care of the salaries of the employees", "Coordinate with suppliers"//Manager's tasks
        };

        string[] TaskDescription =
    {
            "Check which course the orders have chosen for the first course, tell the sous chef what he should prepare and then cook the courses", "Check which course the orders have chosen for the second course, tell the sous chef what he should prepare and then cook the courses",
            "Check which types of desserts the orders have chosen, tell the sous chef what he needs to prepares and cook the desserts", "Check which types of salads the orders have chosen, tell the sous chef what he needs to prepares and cook the salads",//Chef's tasks

            "Prepare for cooking all the vegetables that the chef asked for", "Prepare the pans, pots and ovens for cooking",
            "Prepare the meat that the chef asked for cooking", "Check which of the ingredients the chef asked to prepare are in stock and make a list of all the ingredients that are missing",//Sous chef's tasks

            "Serve the food", "Clear the food from the tables between the courses", "Serve the wine bottles to the tables, open them and pour into glasses",//Waiter's tasks

            "wash the dirty dishes", "Wash the floor after finishing preparing the food", "wipe the wet dishes", "Put the dishes in place after the wiping",//Cleaner's tasks

            "Transport the food from the catering kitchen to the event location", "Return the dishes that belong to the catering kitchen after the end of the event", "Load the food to the truck",//Driver's tasks

            "Coordinates the details of the transaction with the customer", "Collects the payment from the orders", "Ordering the groceries from the grocery list",//Secretary's tasks

            "Transfers the salaries to the employees on the 10th of the month", "Talks with the suppliers and makes sure that there is no shortage of equipment in the kitchen"//Manager's tasks
        };

        string[] TaskDeliverables =
        {
            "First Courses", "Second Courses", "Desserts", "Salads", "Vegetables ready to cook", "Dishes ready for cooking", "Meat ready for cooking",
            "Grocery list", "Ready tables", "Clear tables", "Wine in the glasses", "Clean dishes", "Clean floor", "Dry dishes", "Dishes at place",
            "The food at the event location", "The dishes at the catering kitchen", "The food at the truck", "The transaction details are coordinated with the customer",
            "The transaction payment has been made", "The shoppings has been made", "All the employees got their salaryies",
            "The catering kitchen is equipped with all the necessary dishes"
        };

        string[] TaskRemarks =
        {

        };


        foreach (string task in TaskAlias)
        {
            int i = 0;
            while (task != TaskAlias[i])
                i++;

            DO.WorkerExperience compelexity = 0;
            if (i == 0 || i == 1 || i == 2 || i == 3)
                compelexity = (DO.WorkerExperience)0;
            else if (i == 4 || i == 5 || i == 6 || i == 7)
                compelexity = (DO.WorkerExperience)1;
            else if (i == 8 || i == 9 || i == 10)
                compelexity = (DO.WorkerExperience)2;
            else if (i == 11 || i == 12 || i == 13 || i == 14)
                compelexity = (DO.WorkerExperience)3;
            else if (i == 15 || i == 16 || i == 17)
                compelexity = (DO.WorkerExperience)4;
            else if (i == 18 || i == 19 || i == 20)
                compelexity = (DO.WorkerExperience)5;
            else if (i == 21 || i == 22)
                compelexity = (DO.WorkerExperience)6;


            int WorkerId = 0;//?????

            DateTime createdAtDate = DateTime.Now; 
            DateTime? startDate = null;
            DateTime? scheduledDate = null;
            DateTime? deadlinedate = null;
            DateTime? completeDate = null;
            TimeSpan? RequiredEffortTime = null;

            string deliverables = TaskDeliverables[i];

            bool IsMilestone = false;
        }
    }


    private static void createDependency()
    {

    }

}


/// <param name="Remarks"> Remarks </param>

