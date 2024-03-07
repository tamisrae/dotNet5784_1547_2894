using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Task;

/// <summary>
/// Interaction logic for DependenciesWindow.xaml
/// </summary>
public partial class DependenciesWindow : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();
    int TaskId;

    public DependenciesWindow(int taskId = 0)
    {
        InitializeComponent();
        TaskId = taskId;
        try
        {
            BO.Task? task = bl.Task.Read(taskId);
            TaskList = new List<DependencyTask>();
            List<BO.TaskInList> taskInLists = (bl.Task.ReadAll()).ToList();
            if (task != null && task.Dependencies != null) 
            {
                foreach(BO.TaskInList taskInList in taskInLists)
                {
                    if (task.Dependencies.FirstOrDefault(item => item.Id == taskInList.Id) == null)
                        TaskList.Add(new DependencyTask { Alias = taskInList.Alias, Id = taskInList.Id, Description = taskInList.Description, Status = taskInList.Status, IsDependent = "Add" });
                    else
                        TaskList.Add(new DependencyTask { Alias = taskInList.Alias, Id = taskInList.Id, Description = taskInList.Description, Status = taskInList.Status, IsDependent = "Delete" });
                }
            }
        }
        catch (BlDoesNotExistsException mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }




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
        DependencyTask? dependencyTask=(sender as Button)?.DataContext as DependencyTask;
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
                    BO.TaskInList taskInList = new BO.TaskInList { Alias = dependencyTask.Alias, Id = dependencyTask.Id, Description = dependencyTask.Description, Status = dependencyTask.Status };
                    task.Dependencies.Remove(taskInList);
                    bl.Task.Update(task);
                }
            }

            TaskList = new List<DependencyTask>();
            List<BO.TaskInList> taskInLists = (bl.Task.ReadAll()).ToList();
            if (task != null && task.Dependencies != null)
            {
                foreach (BO.TaskInList taskInList in taskInLists)
                {
                    if (task.Dependencies.FirstOrDefault(item => item.Id == taskInList.Id) == null)
                        TaskList.Add(new DependencyTask { Alias = taskInList.Alias, Id = taskInList.Id, Description = taskInList.Description, Status = taskInList.Status, IsDependent = "Add" });
                    else
                        TaskList.Add(new DependencyTask { Alias = taskInList.Alias, Id = taskInList.Id, Description = taskInList.Description, Status = taskInList.Status, IsDependent = "Delete" });
                }
            }
        }
        catch (Exception mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
