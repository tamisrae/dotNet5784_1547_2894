using BO;
using System.Windows;
using System.Windows.Controls;

namespace PL.Task;

/// <summary>
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();

    public TaskListWindow(BO.WorkerExperience workerExperience = BO.WorkerExperience.Waiter, int taskId = 0, bool isDependency = false)
    {
        IsDependency = isDependency;
        TaskDependencyId = taskId;

        InitializeComponent();

        if (workerExperience == BO.WorkerExperience.Manager && !isDependency)
            TaskList = (bl?.Task.ReadAll()!).ToList();
        else if (workerExperience != BO.WorkerExperience.Manager && !isDependency)
            TaskList = (bl?.Task.ReadAll(item => (int?)item.Complexity == (int)workerExperience)!).ToList();
        else if (isDependency && taskId != 0)
        {
            BO.Task? task = bl.Task.Read(taskId);
            if (task != null && task.Dependencies != null)
            {
                TaskList = new List<BO.TaskInList>();
                foreach (BO.TaskInList dep in task.Dependencies)
                    TaskList.Add(dep);
            }
        }

        ProjectStatus = bl!.ProjectStatusPL();
    }



    public bool IsDependency
    {
        get { return (bool)GetValue(IsDependencyProperty); }
        set { SetValue(IsDependencyProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsDependency.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsDependencyProperty =
        DependencyProperty.Register("IsDependency", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(false));



    public int TaskDependencyId
    {
        get { return (int)GetValue(TaskDependencyIdProperty); }
        set { SetValue(TaskDependencyIdProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskDependencyId.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskDependencyIdProperty =
        DependencyProperty.Register("TaskDependencyId", typeof(int), typeof(TaskListWindow), new PropertyMetadata(0));




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
        TaskList = (Complexity == BO.PLWorkerExperience.All) ?
        (bl?.Task.ReadAll()!).ToList() : (bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Complexity)!).ToList();
    }


    public BO.PLStatus Status { get; set; } = BO.PLStatus.All;

    private void FilterListByStatus(object sender, SelectionChangedEventArgs e)
    {
        TaskList = (Status == BO.PLStatus.All) ?
        (bl?.Task.ReadAll()!).ToList() : (bl?.Task.ReadAll(item => (int?)item.Status == (int)Status)!).ToList();
    }

    private void AddTask(object sender, RoutedEventArgs e)
    {
        new TaskWindow().ShowDialog();
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
                new TaskWindow(task.Id).ShowDialog();
                //update the list of the workers after the changes
                TaskList = (Complexity == BO.PLWorkerExperience.All) ?
                (bl?.Task.ReadAll()!).ToList() : (bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Complexity)!).ToList();
            }
        }
        catch (BlDoesNotExistsException mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }
}
