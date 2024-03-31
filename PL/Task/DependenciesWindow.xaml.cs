using BO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Task;

/// <summary>
/// Interaction logic for DependenciesWindow.xaml
/// </summary>
public partial class DependenciesWindow : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();
    int TaskId, WorkerId;
    BO.WorkerExperience Level = BO.WorkerExperience.Waiter;

    public DependenciesWindow(int taskId = 0, int workerId = 0)
    {
        InitializeComponent();
        TaskId = taskId;
        WorkerId = workerId;
        try
        {
            BO.Task? task = bl.Task.Read(taskId);
            BO.Worker? worker = bl.Worker.Read(workerId);
            if (worker != null)
                Level = worker.Level;

            TaskList = new List<DependencyTask>();
            List<BO.TaskInList> taskInLists = (bl.Task.ReadAll()).ToList();
            if (task != null && task.Dependencies != null)
            {
                foreach (BO.TaskInList taskInList in taskInLists)
                {
                    if (task.Dependencies.FirstOrDefault(item => item.Id == taskInList.Id) == null)
                        TaskList.Add(new DependencyTask { Alias = taskInList.Alias, Id = taskInList.Id, Description = taskInList.Description, Status = taskInList.Status, IsDependent = "Add", ProjectStatus = bl.GetProjectStatus(), Level = Level });
                    else
                        TaskList.Add(new DependencyTask { Alias = taskInList.Alias, Id = taskInList.Id, Description = taskInList.Description, Status = taskInList.Status, IsDependent = "Delete", ProjectStatus = bl.GetProjectStatus(), Level = Level });
                }
            }
        }
        catch (BlDoesNotExistsException mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }


    public BO.ProjectStatus ProjectStatus
    {
        get { return (BO.ProjectStatus)GetValue(ProjectStatusProperty); }
        set { SetValue(ProjectStatusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ProjectStatus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProjectStatusProperty =
        DependencyProperty.Register("ProjectStatus", typeof(BO.ProjectStatus), typeof(DependenciesWindow), new PropertyMetadata(null));



    public List<DependencyTask> TaskList
    {
        get { return (List<DependencyTask>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(List<DependencyTask>), typeof(DependenciesWindow), new PropertyMetadata(null));

    private void AddDeleteDependency(object sender, RoutedEventArgs e)
    {
        DependencyTask? dependencyTask = (sender as Button)?.DataContext as DependencyTask;
        try
        {
            BO.Task? task = bl.Task.Read(TaskId);
            if (dependencyTask != null && task != null)
            {
                if (dependencyTask.IsDependent == "Add" && task.Dependencies != null)
                {
                    task.Dependencies.Add(new BO.TaskInList { Alias = dependencyTask.Alias, Id = dependencyTask.Id, Description = dependencyTask.Description, Status = dependencyTask.Status });
                    bl.Task.Update(task);
                }
                else if (dependencyTask.IsDependent == "Add" && task.Dependencies == null)
                {
                    task.Dependencies = new List<BO.TaskInList>();
                    task.Dependencies.Add(new BO.TaskInList { Alias = dependencyTask.Alias, Id = dependencyTask.Id, Description = dependencyTask.Description, Status = dependencyTask.Status });
                    bl.Task.Update(task);
                }
                else if (dependencyTask.IsDependent == "Delete" && task.Dependencies != null)
                {
                    foreach (var dependency in task.Dependencies)
                    {
                        if (dependency.Id == dependencyTask.Id)
                        {
                            task.Dependencies.Remove(dependency);
                            bl.Task.Update(task);
                            break;
                        }
                    }
                }
            }

            TaskList = new List<DependencyTask>();
            List<BO.TaskInList> taskInLists = (bl.Task.ReadAll()).ToList();
            if (task != null && task.Dependencies != null)
            {
                foreach (BO.TaskInList taskInList in taskInLists)
                {
                    if (task.Dependencies.FirstOrDefault(item => item.Id == taskInList.Id) == null)
                        TaskList.Add(new DependencyTask { Alias = taskInList.Alias, Id = taskInList.Id, Description = taskInList.Description, Status = taskInList.Status, IsDependent = "Add", ProjectStatus = bl.GetProjectStatus() });
                    else
                        TaskList.Add(new DependencyTask { Alias = taskInList.Alias, Id = taskInList.Id, Description = taskInList.Description, Status = taskInList.Status, IsDependent = "Delete", ProjectStatus = bl.GetProjectStatus() });
                }
            }
        }
        catch (Exception mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ShowTaskClick(object sender, MouseButtonEventArgs e)
    {
        DependencyTask? dependencyTask = (sender as ListView)?.SelectedItem as DependencyTask;
        BO.Task? task = null;
        try
        {
            if (dependencyTask != null)
                task = bl.Task.Read(dependencyTask.Id);
            if (task != null)
                new TaskWindow(task.Id, WorkerId).ShowDialog();
        }
        catch (BlDoesNotExistsException mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }
}
