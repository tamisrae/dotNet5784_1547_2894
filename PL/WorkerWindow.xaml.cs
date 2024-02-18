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

namespace PL
{
    /// <summary>
    /// Interaction logic for WorkerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        static readonly BlApi.IBl bl = BlApi.Factory.Get();
        int ID;
        public WorkerWindow(int Id = 0)
        {
            InitializeComponent();
            ID = Id;
            BO.Worker? worker;
            if (Id == 0)
                worker = new BO.Worker { Id = 0, Level = (BO.WorkerExperience)7, Email = "", Cost = 0, Name = "", CurrentTask = null };
            else
            {
                try
                {
                    worker = bl.Worker.Read(Id);
                }
                catch(BlDoesNotExistsException)
                {
                    MessageBox.Show($"Worker with ID={Id} does not exists", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public BO.Worker? Worker { get; set; }



        public BO.Worker CurrentWorker
        {
            get { return (BO.Worker)GetValue(WorkerProperty); }
            set { SetValue(WorkerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Worker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WorkerProperty =
            DependencyProperty.Register("Worker", typeof(BO.Worker), typeof(MainWindow), new PropertyMetadata(null));

        private void AddUpdateClick(object sender, RoutedEventArgs e)
        {
            BO.Worker? worker = sender as BO.Worker;
            if (worker != null)
            {
                try
                {
                    if (ID != 0) 
                    {
                        bl.Worker.Update(worker);
                        MessageBox.Show("The worker was successfully updated", "UPDATE", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        bl.Worker.Create(worker);
                        MessageBox.Show("The worker was successfully added", "ADD", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                catch (BlDoesNotExistsException)
                {
                    MessageBox.Show($"Worker with ID={worker.Id} does not exists", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlAlreadyExistsException)
                {
                    MessageBox.Show($"Worker with ID={worker.Id} already exists", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
