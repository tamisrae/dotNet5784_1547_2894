using BO;
using System;
using System.Collections.Generic;
using System.Data;
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
    int taskID;
    int workerID;

    public TaskWindow(int taskId = 0, int workerId = 0)
    {
        taskID = taskId;
        workerID = workerId;
        if (taskId == 0)//create new worker window with default values
            CurrentTask = new BO.Task { Id = 0, Alias = "", Description = "" };
        else//create new worker window with the worker's data
        {
            try
            {
                CurrentTask = bl.Task.Read(taskId)!;
            }
            catch (BlDoesNotExistsException mess)
            {
                CurrentTask = null!;
                MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        ProjectStatus = bl.ProjectStatusPL();

        //if (workerId != 0)
        //{
        //    //BO.Worker? worker = bl.Worker.Read(workerId);
        //    BO.TaskInList? taskInList = bl.Task.ReadAll().FirstOrDefault(item => bl.Task.Read(item.Id)!.WorkOnTask != null && bl.Task.Read(item.Id)!.WorkOnTask!.Id == workerId && bl.Task.Read(item.Id)!.Status == BO.Status.OnTrack);
        //    if (taskInList != null)
        //        CurrentTask = bl.Task.Read(taskInList.Id);
        //    CurrentTask = null;
        //}

        InitializeComponent();
    }


    public BO.Task? CurrentTask
    {
        get { return (BO.Task?)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));




    public BO.ProjectStatus ProjectStatus
    {
        get { return (BO.ProjectStatus)GetValue(ProjectStatusProperty); }
        set { SetValue(ProjectStatusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ProjectStatus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProjectStatusProperty =
        DependencyProperty.Register("ProjectStatus", typeof(BO.ProjectStatus), typeof(TaskWindow), new PropertyMetadata(null));



    private void AddUpdateTask(object sender, RoutedEventArgs e)
    {
        this.Close();
        if (CurrentTask != null)
        {
            try
            {
                if (taskID != 0)//if the id is not 0 it means that we need to update the data
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

    private void DeleteTaskClick(object sender, RoutedEventArgs e)
    {
        this.Close();
        if (CurrentTask != null)
        {
            try
            {
                bl.Worker.Delete(CurrentTask.Id);
                MessageBox.Show("The task was successfully deleted", "DELETE", MessageBoxButton.OK);
                CurrentTask = null!;
            }
            catch (Exception mess)
            {
                MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }

    private void ShowDependenciesClick(object sender, RoutedEventArgs e)
    {
        if (CurrentTask != null)
            new DependenciesWindow(CurrentTask.Id).ShowDialog();
        try
        {
            if (CurrentTask != null)
                CurrentTask = bl.Task.Read(CurrentTask.Id);
        }
        catch (Exception mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }

    private void SignUpForTaskClick(object sender, RoutedEventArgs e)
    {
        try
        {
            bl.Task.SignUpForTask(taskID, workerID);
            CurrentTask = bl.Task.Read(taskID);
        }
        catch (Exception mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }

    private void StartTask(object sender, RoutedEventArgs e)
    {
        try
        {
            BO.Task? task = bl.Task.Read(taskID);
            if (task != null)
            {
                bl.Task.StartTask(task, workerID);
            }
            CurrentTask = bl.Task.Read(taskID);
        }
        catch (Exception mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }

    private void EndTask(object sender, RoutedEventArgs e)
    {
        try
        {
            bl.Task.EndTask(taskID, workerID);
            CurrentTask = bl.Task.Read(taskID);
        }
        catch (Exception mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }
}
