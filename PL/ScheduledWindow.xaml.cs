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
    /// Interaction logic for ScheduledWindow.xaml
    /// </summary>
    public partial class ScheduledWindow : Window
    {
        static readonly BlApi.IBl bl = BlApi.Factory.Get();

        public ScheduledWindow()
        {
            InitializeComponent();
        }



        public DateTime StartProjectDate
        {
            get { return (DateTime)GetValue(StartProjectDateProperty); }
            set { SetValue(StartProjectDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartProjectDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartProjectDateProperty =
            DependencyProperty.Register("StartProjectDate", typeof(DateTime), typeof(ScheduledWindow), new PropertyMetadata(null));

        private void SetStartProjectDate(object sender, RoutedEventArgs e)
        {
            
            bl.Task.AutomaticSchedule();
        }
    }
}
