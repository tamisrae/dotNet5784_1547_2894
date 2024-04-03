using BO;
using DO;
using System.Windows;
using System.Windows.Controls;

namespace PL.Task;

/// <summary>
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();
    int workerID;

    public TaskListWindow(BO.WorkerExperience workerExperience = BO.WorkerExperience.Waiter, int taskId = 0, int workerId = 0)
    {
        Level = workerExperience;
        workerID = workerId;
        ProjectStatus = bl!.GetProjectStatus();

        InitializeComponent();


        if (workerExperience == BO.WorkerExperience.Manager)
            TaskList = (bl?.Task.ReadAll()!).ToList();
        else if (workerExperience != BO.WorkerExperience.Manager)
            TaskList = (bl?.Task.ReadAll(item => (int?)item.Complexity == (int)workerExperience && (item.WorkOnTask == null || item.WorkOnTask.Id == workerID) && item.ScheduledDate != null)!).ToList();
    }


 
    public BO.WorkerExperience Level
    {
        get { return (BO.WorkerExperience)GetValue(LevelProperty); }
        set { SetValue(LevelProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Level.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LevelProperty =
        DependencyProperty.Register("Level", typeof(BO.WorkerExperience), typeof(TaskListWindow), new PropertyMetadata(BO.WorkerExperience.Waiter));




    public List<BO.TaskInList> TaskList
    {
        get { return (List<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(List<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));



    public BO.ProjectStatus ProjectStatus
    {
        get { return (BO.ProjectStatus)GetValue(ProjectStatusProperty); }
        set { SetValue(ProjectStatusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ProjectStatus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProjectStatusProperty =
        DependencyProperty.Register("ProjectStatus", typeof(BO.ProjectStatus), typeof(TaskListWindow), new PropertyMetadata(null));




    public BO.PLWorkerExperience Complexity { get; set; } = BO.PLWorkerExperience.All;

    private void FilterListByComplexity(object sender, SelectionChangedEventArgs e)
    {
        if (Complexity == BO.PLWorkerExperience.All && Status == BO.PLStatus.All)
            TaskList = (bl?.Task.ReadAll()!).ToList();
        else if(Complexity == BO.PLWorkerExperience.All && Status != BO.PLStatus.All)
            TaskList = (bl?.Task.ReadAll(item => (int?)item.Status == (int)Status)!).ToList();
        else if(Complexity != BO.PLWorkerExperience.All && Status == BO.PLStatus.All)
            TaskList = (bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Complexity)!).ToList();
        else
            TaskList = (bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Complexity && (int?)item.Status == (int)Status)!).ToList();
    }


    public BO.PLStatus Status { get; set; } = BO.PLStatus.All;

    private void FilterListByStatus(object sender, SelectionChangedEventArgs e)
    {
        if (Complexity == BO.PLWorkerExperience.All && Status == BO.PLStatus.All)
            TaskList = (bl?.Task.ReadAll()!).ToList();
        else if (Complexity == BO.PLWorkerExperience.All && Status != BO.PLStatus.All)
            TaskList = (bl?.Task.ReadAll(item => (int?)item.Status == (int)Status)!).ToList();
        else if (Complexity != BO.PLWorkerExperience.All && Status == BO.PLStatus.All)
            TaskList = (bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Complexity)!).ToList();
        else
            TaskList = (bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Complexity && (int?)item.Status == (int)Status)!).ToList();
    }

    private void AddTask(object sender, RoutedEventArgs e)
    {
        new TaskWindow(0, workerID).ShowDialog();
        //update the list of the tasks after the changes
        TaskList = (Complexity == BO.PLWorkerExperience.All) ?
        (bl?.Task.ReadAll()!).ToList() : (bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Complexity)!).ToList();
    }

    private void UpdateTask(object sender, RoutedEventArgs e)
    {
        BO.TaskInList? taskInList = (sender as ListView)?.SelectedItem as BO.TaskInList;
        BO.Task? task = null;
        try
        {
            if (taskInList != null)
                task = bl.Task.Read(taskInList.Id);
            if (task != null)
            {
                new TaskWindow(task.Id, workerID).ShowDialog();
                //update the list of the tasks after the changes
                if (Level == BO.WorkerExperience.Manager)
                    TaskList = (Complexity == BO.PLWorkerExperience.All) ?
                    (bl?.Task.ReadAll()!).ToList() : (bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Complexity)!).ToList();
                else
                    TaskList = (bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Level && (item.WorkOnTask == null || item.WorkOnTask.Id == workerID) && item.ScheduledDate != null)!).ToList();
            }
        }
        catch (BlDoesNotExistsException mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }
}
