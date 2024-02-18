using System.Windows;
using System.Windows.Controls;

namespace PL;

/// <summary>
/// Interaction logic for WorkerListWindow.xaml
/// </summary>
public partial class WorkerListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public WorkerListWindow()
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

    public BO.WorkerExperience Experience { get; set; } = BO.WorkerExperience.All;

    private void ListFliterByExperince(object sender, SelectionChangedEventArgs e)
    {
        WorkersList = (Experience == BO.WorkerExperience.All) ?
        s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.Level == Experience)!;
    }

    private void AddWorker(object sender, RoutedEventArgs e)
    {
        new WorkerWindow().ShowDialog();
    }
    
    private void DoubleClick (object sender, RoutedEventArgs e) 
    {
        BO.Worker? worker = (sender as ListView)?.SelectedItem as BO.Worker;
        if (worker != null)
        {
            new WorkerWindow(worker.Id).ShowDialog();
            WorkersList = (Experience == BO.WorkerExperience.All) ?
            s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.Level == Experience)!;
        }
    }
}





