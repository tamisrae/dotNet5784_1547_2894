using BlApi;
using BO;
using PL.Task;
using PL.User;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl bl = BlApi.Factory.Get();

        public MainWindow(int Id = 0)
        {
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



        public BO.Worker? CurrentWorker
        {
            get { return (BO.Worker?)GetValue(CurrentWorkerProperty); }
            set { SetValue(CurrentWorkerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentWorker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentWorkerProperty =
            DependencyProperty.Register("CurrentWorker", typeof(BO.Worker), typeof(MainWindow), new PropertyMetadata(null));



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
                new TaskListWindow(CurrentWorker.Level).ShowDialog();
        }

        private void ScheduledDate(object sender, RoutedEventArgs e)
        {
           new ScheduledWindow().ShowDialog();
        }

        private void GantClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("wish us luck", "yalla", MessageBoxButton.OK);
        }

        private void CurrentTaskClick(object sender, RoutedEventArgs e)
        {
            if (CurrentWorker != null)
                new TaskWindow(0, CurrentWorker.Id).ShowDialog();
        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            if (CurrentWorker != null)
                new UserWindow(CurrentWorker.Id).ShowDialog();
        }
    }
}