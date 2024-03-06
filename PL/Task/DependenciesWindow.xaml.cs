using BO;
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

    public DependenciesWindow(int taskId = 0)
    {
        InitializeComponent();
        TaskId = taskId;
        try
        {
            BO.Task? task;
            DependenciesList = new List<BO.TaskInList>();
            if (taskId != 0)
            {
                task = bl.Task.Read(taskId);
                if (task != null && task.Dependencies != null)
                {
                    foreach (BO.TaskInList dep in task.Dependencies)
                        DependenciesList.Add(dep);
                }
            }
        }
        catch (BlDoesNotExistsException mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }



    public int TaskId
    {
        get { return (int)GetValue(TaskIdProperty); }
        set { SetValue(TaskIdProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskId.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskIdProperty =
        DependencyProperty.Register("TaskId", typeof(int), typeof(DependenciesWindow), new PropertyMetadata(0));





    public List<BO.TaskInList> DependenciesList
    {
        get { return (List<BO.TaskInList>)GetValue(DependenciesListProperty); }
        set { SetValue(DependenciesListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for DependenciesList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DependenciesListProperty =
        DependencyProperty.Register("DependenciesList", typeof(List<BO.TaskInList>), typeof(DependenciesWindow), new PropertyMetadata(null));

    private void task_Click(object sender, RoutedEventArgs e)
    {
        BO.TaskInList? taskInList = (sender as Button)?.DataContext as BO.TaskInList;
    }
}
