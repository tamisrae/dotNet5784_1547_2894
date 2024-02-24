using System;
using System.Collections.Generic;
using System.Linq;
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
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();

    public TaskListWindow()
    {
        InitializeComponent();

        TasksList = bl.Task.ReadAll();
    }

    public IEnumerable<BO.TaskInList> TasksList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TasksListProperty); }
        set { SetValue(TasksListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskInList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TasksListProperty =
        DependencyProperty.Register("TasksList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));


    public BO.PLWorkerExperience Complexity { get; set; } = BO.PLWorkerExperience.All;

    private void FilterListByComplexity(object sender, SelectionChangedEventArgs e)
    {
        TasksList = (Complexity == BO.PLWorkerExperience.All) ?
       bl?.Task.ReadAll()! : bl?.Task.ReadAll(item => (int?)item.Complexity == (int)Complexity)!;
    }
}
