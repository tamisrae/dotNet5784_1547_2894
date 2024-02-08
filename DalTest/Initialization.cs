
namespace DalTest;
using DalApi;
using DO;
using System.Net.Http.Headers;

public static class Initialization
{
    //private static IWorker? s_dalWorker; //stage 1
    //private static ITask? s_dalTask; //stage 1
    //private static IDependency? s_dalDependency; //stage 1
    private static IDal? s_dal; //stage 2

    private static readonly Random s_rand = new();

    /// <summary>
    /// Create workers and push them to the list
    /// </summary>
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
            int minWorkerId = 200000000, maxWorkerId = 400000000;

            int workerId = s_rand.Next(minWorkerId, maxWorkerId);
           

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
            s_dal!.Worker.Create(newWorker);
        }
    }

    /// <summary>
    /// Create tasks and push them to the list
    /// </summary>
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
            "Don't forget to add salt to the first course", "Don't forget to add salt to the second course", "Don't forget to melt chocolate before serving" ,
            "Chop the onion finely", "cut them chopped", "Be careful not to burn the oil", "Don't forget to sharpen the knife", "Only what is really necessary",
            "Don't forget to smile at the customer and bring a tooth pices", "Don't forget to wipe the tables", "Do not leave the cork on the diner's table",
            "Don't forget to use hot water for greasy dishes","Don't forget before to pick things up from the floor", "Don't forget to wipe inside the glass",
            "Put every tool in its place and do not change the order", "Don't forget to turn on the cooling in the truck", "Be careful not to break dishes when you bring them back", 
            "Be careful not to spill the food in the car", "Don't forget to ask what event it is", "Ask whether to split into payments", "Check that there are no duplicates", 
            "Don't forget to add bonuses", "Don't forget to talk to the chef to know what equipment is missing",

        };

        

        foreach (string task in TaskAlias)
        {
            int i = 0;
            while (task != TaskAlias[i])
                i++;

            DO.WorkerExperience complexity = 0;
            if (i == 0 || i == 1 || i == 2 || i == 3)
                complexity = (DO.WorkerExperience)0;
            else if (i == 4 || i == 5 || i == 6 || i == 7)
                complexity = (DO.WorkerExperience)1;
            else if (i == 8 || i == 9 || i == 10)
                complexity = (DO.WorkerExperience)2;
            else if (i == 11 || i == 12 || i == 13 || i == 14)
                complexity = (DO.WorkerExperience)3;
            else if (i == 15 || i == 16 || i == 17)
                complexity = (DO.WorkerExperience)4;
            else if (i == 18 || i == 19 || i == 20)
                complexity = (DO.WorkerExperience)5;
            else if (i == 21 || i == 22)
                complexity = (DO.WorkerExperience)6;
            

            Random rand = new Random(DateTime.Now.Millisecond);
            DateTime start = new DateTime(2024, 2, 8, 0, 0, 0);
            int rangeStart = (start - DateTime.Today).Days;
            DateTime ranDay = start.AddDays(rand.Next(rangeStart));

            DateTime createdAtDate = ranDay; 
            DateTime? startDate = null;
            DateTime? scheduledDate = null;
            DateTime? deadlinedate = null;
            DateTime? completeDate = null;
            TimeSpan? requiredEffortTime = null;

            //rand = new Random(TimeSpan.FromDays(3));
            
            string deliverables = TaskDeliverables[i];
            string description = TaskDescription[i];
            string remarks = TaskRemarks[i];

            bool isMilestone = false;

            Task newTask = new Task(task, description, createdAtDate, isMilestone, 0, complexity, null,
                                   requiredEffortTime, startDate, scheduledDate, deadlinedate, completeDate, deliverables, remarks);

            s_dal!.Task.Create(newTask);
        }
    }

    /// <summary>
    /// Create dependencies and push them to the list
    /// </summary>
    private static void createDependency()
    {
        s_dal!.Dependency.Create(new Dependency(0, 1, 19));
        s_dal!.Dependency.Create(new Dependency(0, 1, 23));
        s_dal!.Dependency.Create(new Dependency(0, 2, 19));
        s_dal!.Dependency.Create(new Dependency(0,2 , 23));
        s_dal!.Dependency.Create(new Dependency(0, 3, 23));
        s_dal!.Dependency.Create(new Dependency(0, 3, 19));
        s_dal!.Dependency.Create(new Dependency(0, 4, 23));
        s_dal!.Dependency.Create(new Dependency(0, 4, 19));
        s_dal!.Dependency.Create(new Dependency(0, 5, 4));
        s_dal!.Dependency.Create(new Dependency(0, 6, 2));
        s_dal!.Dependency.Create(new Dependency(0, 6, 1));
        s_dal!.Dependency.Create(new Dependency(0, 7, 2));
        s_dal!.Dependency.Create(new Dependency(0, 7, 1));
        s_dal!.Dependency.Create(new Dependency(0, 8, 1));
        s_dal!.Dependency.Create(new Dependency(0, 8, 2));
        s_dal!.Dependency.Create(new Dependency(0, 8, 3));
        s_dal!.Dependency.Create(new Dependency(0, 8, 4));
        s_dal!.Dependency.Create(new Dependency(0, 9, 1));
        s_dal!.Dependency.Create(new Dependency(0, 9, 2));
        s_dal!.Dependency.Create(new Dependency(0, 9, 3));
        s_dal!.Dependency.Create(new Dependency(0, 9, 4));
        s_dal!.Dependency.Create(new Dependency(0, 9, 16));
        s_dal!.Dependency.Create(new Dependency(0, 10, 9));
        s_dal!.Dependency.Create(new Dependency(0, 11, 9));
        s_dal!.Dependency.Create(new Dependency(0, 12, 17));
        s_dal!.Dependency.Create(new Dependency(0, 13, 12));
        s_dal!.Dependency.Create(new Dependency(0, 13, 16));
        s_dal!.Dependency.Create(new Dependency(0, 14, 12));
        s_dal!.Dependency.Create(new Dependency(0, 14, 17));
        s_dal!.Dependency.Create(new Dependency(0, 15, 12));
        s_dal!.Dependency.Create(new Dependency(0, 15, 14));
        s_dal!.Dependency.Create(new Dependency(0, 16, 18));
        s_dal!.Dependency.Create(new Dependency(0, 17, 9));
        s_dal!.Dependency.Create(new Dependency(0, 17, 10));
        s_dal!.Dependency.Create(new Dependency(0, 18, 1));
        s_dal!.Dependency.Create(new Dependency(0, 18, 2));
        s_dal!.Dependency.Create(new Dependency(0, 18, 3));
        s_dal!.Dependency.Create(new Dependency(0, 18, 4));
        s_dal!.Dependency.Create(new Dependency(0, 20, 19));
        s_dal!.Dependency.Create(new Dependency(0, 21, 8));
        s_dal!.Dependency.Create(new Dependency(0, 23, 22));
    }

    //public static void Do(IDal dal) //stage 2
    public static void Do() //stage 4
    {
        //s_dal = dal ?? throw new NullReferenceException("DAL can not be null!");//stage 2
        //s_dal = dal ?? throw new NullReferenceException("DAL can not be null!");//stage 2
        //s_dal = dal ?? throw new NullReferenceException("DAL can not be null!");//stage 2
        s_dal = DalApi.Factory.Get; //stage 4

        createWorker();
        createTask();
        createDependency();
    }
}