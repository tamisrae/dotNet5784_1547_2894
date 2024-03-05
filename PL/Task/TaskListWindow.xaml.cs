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

    public TaskListWindow(BO.WorkerExperience workerExperience = BO.WorkerExperience.Waiter)
    {
        if (workerExperience == BO.WorkerExperience.Manager)
            TaskList = bl?.Task.ReadAll()!;
        else
            TaskList = bl?.Task.ReadAll(item => (int?)item.Complexity == (int)workerExperience)!;
        ProjectStatus = bl!.ProjectStatusPL();

        InitializeComponent();
    }

    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));



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
        bl?.Task.ReadAll()! : bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Complexity)!;
    }


    public BO.PLStatus Status { get; set; } = BO.PLStatus.All;

    private void FilterListByStatus(object sender, SelectionChangedEventArgs e)
    {
        TaskList = (Status == BO.PLStatus.All) ?
        bl?.Task.ReadAll()! : bl?.Task.ReadAll(item => (int?)item.Status == (int)Status)!;
    }

    private void AddTask(object sender, RoutedEventArgs e)
    {
        new TaskWindow().ShowDialog();
        //update the list of the tasks after the changes
        TaskList = (Complexity == BO.PLWorkerExperience.All) ?
        bl?.Task.ReadAll()! : bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Complexity)!;
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
                bl?.Task.ReadAll()! : bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Complexity)!;
            }
        }
        catch (BlDoesNotExistsException mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }
}
