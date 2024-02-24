using System.Windows;
using System.Windows.Controls;

namespace PL;

/// <summary>
/// Interaction logic for WorkerListWindow.xaml
/// </summary>
public partial class WorkerListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public WorkerListWindow()//ctor  
    {
        InitializeComponent();
        WorkersList = s_bl?.Worker.ReadAll()!;
    }


    public IEnumerable<BO.Worker> WorkersList
    {
        get { return (IEnumerable<BO.Worker>)GetValue(WorkersListProperty); }
        set { SetValue(WorkersListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for WorkersList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty WorkersListProperty =
        DependencyProperty.Register("WorkersList", typeof(IEnumerable<BO.Worker>), typeof(WorkerListWindow), new PropertyMetadata(null));

    public BO.PLWorkerExperience Experience { get; set; } = BO.PLWorkerExperience.All;

    private void ListFliterByExperince(object sender, SelectionChangedEventArgs e)//filter the list by the experience
    {
        WorkersList = (Experience == BO.PLWorkerExperience.All) ?
        s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => (int)item.Level == (int)Experience)!;
    }

    private void AddWorker(object sender, RoutedEventArgs e)//when the user want to add a worker
    {
        new WorkerWindow().ShowDialog();
        //update the list of the workers after the changes
        WorkersList = (Experience == BO.PLWorkerExperience.All) ?
        s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => (int)item.Level == (int)Experience)!;
    }
    
    private void WorkerDoubleClick (object sender, RoutedEventArgs e) //when the user click on a worker
    {
        BO.Worker? worker = (sender as ListView)?.SelectedItem as BO.Worker;
        if (worker != null)
        {
            new WorkerWindow(worker.Id).ShowDialog();
            //update the list of the workers after the changes
            WorkersList = (Experience == BO.PLWorkerExperience.All) ?
            s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => (int)item.Level == (int)Experience)!;
        }
    }
}





