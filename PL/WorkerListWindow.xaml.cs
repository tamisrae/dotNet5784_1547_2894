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


    }
}
