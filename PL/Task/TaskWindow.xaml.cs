using BO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
/// Interaction logic for TaskWindow.xaml
/// </summary>
public partial class TaskWindow : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();
    int ID;

    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));


    public TaskWindow(int Id = 0)
    {
        InitializeComponent();
        ID = Id;
        TaskList = bl.Task.ReadAll();

        if (Id == 0)//create new worker window with default values
            CurrentTask = new BO.Task { Id = 0, Alias = "", Description = "" };
        else//create new worker window with the worker's data
        {
            try
            {
                CurrentTask = bl.Task.Read(Id)!;
            }
            catch (BlDoesNotExistsException mess)
            {
                CurrentTask = null!;
                MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }


    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

    private void AddUpdateTask(object sender, RoutedEventArgs e)
    {
        this.Close();
        if (CurrentTask != null)
        {
            try
            {
                if (ID != 0)//if the id is not 0 it means that we need to update the data
                {
                    bl.Task.Update(CurrentTask);
                    MessageBox.Show("The task was successfully updated", "UPDATE", MessageBoxButton.OK);
                }
                else//if the id is 0 it means that we need to creat new worker
                {
                    bl.Task.Create(CurrentTask);
                    MessageBox.Show("The task was successfully added", "ADD", MessageBoxButton.OK);
                }
            }
            catch (BlDoesNotExistsException mess)
            {
                MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (BlAlreadyExistsException mess)
            {
                MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (Exception mess)
            {
                MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }

    private void OpenTaskWindow(object sender, MouseButtonEventArgs e)
    {
        BO.TaskInList? taskInList = (sender as ListBox)?.SelectedItem as BO.TaskInList;
        BO.Task? task = null;
        try
        {
            if (taskInList != null)
                task = bl.Task.Read(taskInList.Id);
            if (task != null)
                new TaskWindow(task.Id).ShowDialog();
        }
        catch (BlDoesNotExistsException mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }




  


}
