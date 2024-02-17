using BlApi;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WorkersListShow(object sender, RoutedEventArgs e)
        {
            new WorkerListWindow().Show();

        }

        private void Initialization(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do yoy want to initialize the data?", "initializing data", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Factory.Get().InitializeDB();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        private void ResetData(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do yoy want to reset the data?", "reseting data", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Factory.Get().ResetDB();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }
    }
}