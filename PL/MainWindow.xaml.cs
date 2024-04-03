using BlApi;
using BO;
using PL.Task;
using PL.User;
using System.ComponentModel;
using System.Windows;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();
    public MainWindow(int Id = 0)
    {
        Activated += MainWindow_Activated;
        try
        {
            CurrentWorker = bl.Worker.Read(Id);
        }
        catch (BlDoesNotExistsException mess)
        {
            CurrentWorker = null!;
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
        InitializeComponent();
    }

    private void MainWindow_Activated(object? sender, EventArgs e)
    {
        CurrentTime = bl.Clock;
    }

    public BO.Worker? CurrentWorker
    {
        get { return (BO.Worker?)GetValue(CurrentWorkerProperty); }
        set { SetValue(CurrentWorkerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentWorker.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentWorkerProperty =
        DependencyProperty.Register("CurrentWorker", typeof(BO.Worker), typeof(MainWindow), new PropertyMetadata(null));

    public DateTime CurrentTime
    {
        get { return (DateTime)GetValue(CurrentTimeProperty); }
        set { SetValue(CurrentTimeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTimeProperty =
        DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(bl.Clock));

    private void WorkersListShow(object sender, RoutedEventArgs e)
    {
        new WorkerListWindow().ShowDialog();
    }

    private void Initialization(object sender, RoutedEventArgs e)
    {
        //ask the user if they want to initialize the data
        MessageBoxResult result = MessageBox.Show("Do you want to initialize the data?", "initializing data", MessageBoxButton.YesNo, MessageBoxImage.Question);
        try
        {
            switch (result)
            {
                case MessageBoxResult.Yes://if they want
                    Factory.Get().InitializeDB();
                    break;
                case MessageBoxResult.No://if they dont want
                    break;
                default:
                    break;
            }
        }
        catch (Exception mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }

    private void ResetData(object sender, RoutedEventArgs e)
    {
        //ask the user if they want to reset the data
        MessageBoxResult result = MessageBox.Show("Do you want to reset the data?", "reseting data", MessageBoxButton.YesNo, MessageBoxImage.Question);
        try
        {
            switch (result)
            {
                case MessageBoxResult.Yes://if they want
                    Factory.Get().ResetDB();
                    break;
                case MessageBoxResult.No://if they dont want
                    break;
                default:
                    break;
            }
        }
        catch (Exception mess)
        {
            MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }

    private void TasksListShow(object sender, RoutedEventArgs e)
    {
        if (CurrentWorker != null)
            new TaskListWindow(CurrentWorker.Level, 0, CurrentWorker.Id).ShowDialog();
    }

    private void ScheduledDate(object sender, RoutedEventArgs e)
    {
        new ScheduledWindow().ShowDialog();
    }

    private void GantClick(object sender, RoutedEventArgs e)
    {
       new GantWindow().ShowDialog();
    }

    private void CurrentTaskClick(object sender, RoutedEventArgs e)
    {
        if (CurrentWorker != null)
        {
            BO.TaskInList? taskInList = bl.Task.ReadAll().FirstOrDefault(item => bl.Task.Read(item.Id)!.WorkOnTask != null && bl.Task.Read(item.Id)!.WorkOnTask!.Id == CurrentWorker.Id && bl.Task.Read(item.Id)!.Status == BO.Status.OnTrack)!;
            if (taskInList != null)
                new TaskWindow(taskInList.Id, CurrentWorker.Id).ShowDialog();
            else
                MessageBox.Show("You are not working on any tasks at the moment", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void UserClick(object sender, RoutedEventArgs e)
    {
        if (CurrentWorker != null)
            new UserWindow(CurrentWorker.Id).ShowDialog();
    }

    private void MonthButton(object sender, RoutedEventArgs e)
    {
        bl.AdvanceTimeByMonth();
        CurrentTime = bl.Clock;
    }

    private void DayButton(object sender, RoutedEventArgs e)
    {
        bl.AdvanceTimeByDay();
        CurrentTime = bl.Clock;
    }

    private void YearButton(object sender, RoutedEventArgs e)
    {
        bl.AdvanceTimeByYear();
        CurrentTime = bl.Clock;
    }

    private void HourButton(object sender, RoutedEventArgs e)
    {
        bl.AdvanceTimeByHour();
        CurrentTime = bl.Clock;
    }

    private void ResetTimeButton(object sender, RoutedEventArgs e)
    {
        bl.ResetTime();
        CurrentTime = bl.Clock;
    }
}
