using BlApi;
using BO;
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

namespace PL;

/// <summary>
/// Interaction logic for WorkerWindow.xaml
/// </summary>
public partial class WorkerWindow : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();
    int ID;


    public BO.Worker CurrentWorker
    {
        get { return (BO.Worker)GetValue(CurrentWorkerProperty); }
        set { SetValue(CurrentWorkerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentWorker.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentWorkerProperty =
        DependencyProperty.Register("CurrentWorker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));


    public BO.TaskInWorker CurrentTask
    {
        get { return (BO.TaskInWorker)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.TaskInWorker), typeof(WorkerWindow), new PropertyMetadata(null));



    public WorkerWindow(int Id = 0)//ctor
    {
        ID = Id;
        if (Id == 0)//create new worker window with default values
            CurrentWorker = new BO.Worker { Id = 0, Level = (BO.WorkerExperience)7, Email = "", Cost = 0, Name = "", CurrentTask = null };
        else//create new worker window with the worker's data
        {
            try
            {
                CurrentWorker = bl.Worker.Read(Id)!;
            }
            catch(BlDoesNotExistsException mess)
            {
                CurrentWorker = null!;
                MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        InitializeComponent();
    }



    private void AddUpdateClick(object sender, RoutedEventArgs e)
    {
        this.Close();
        if (CurrentWorker != null)
        {
            try
            {
                if (ID != 0)//if the id is not 0 it means that we need to update the data
                {
                    bl.Worker.Update(CurrentWorker);
                    MessageBox.Show("The worker was successfully updated", "UPDATE", MessageBoxButton.OK);
                }
                else//if the id is 0 it means that we need to creat new worker
                {
                    bl.Worker.Create(CurrentWorker);
                    MessageBox.Show("The worker was successfully added", "ADD", MessageBoxButton.OK);
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
            catch(Exception mess)
            {
                MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }

    private void DeleteClick(object sender, RoutedEventArgs e)
    {
        this.Close();
        if (CurrentWorker != null) 
        {
            try
            {
                bl.Worker.Delete(CurrentWorker.Id);
                MessageBox.Show("The worker was successfully deleted", "DELETE", MessageBoxButton.OK);
                CurrentWorker = null!;
            }
            catch (Exception mess)
            {
                MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }
}
