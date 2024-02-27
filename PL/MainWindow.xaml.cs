using BlApi;
using PL.Task;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl bl = BlApi.Factory.Get();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WorkersListShow(object sender, RoutedEventArgs e)
        {
            new WorkerListWindow().ShowDialog();
        }

        private void Initialization(object sender, RoutedEventArgs e)
        {
            //ask the user if they want to initialize the data
            MessageBoxResult result = MessageBox.Show("Do you want to initialize the data?", "initializing data", MessageBoxButton.YesNo, MessageBoxImage.Question);
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

        private void ResetData(object sender, RoutedEventArgs e)
        {
            //ask the user if they want to reset the data
            MessageBoxResult result = MessageBox.Show("Do you want to reset the data?", "reseting data", MessageBoxButton.YesNo, MessageBoxImage.Question);
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

        private void TasksListShow(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().ShowDialog();
        }

        private void ScheduledDate(object sender, RoutedEventArgs e)
        {
           new ScheduledWindow().ShowDialog();
        }
    }
}