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
/// Interaction logic for FirstWindow.xaml
/// </summary>
public partial class FirstWindow : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();

    public int ID
    {
        get { return (int)GetValue(IDProperty); }
        set { SetValue(IDProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ID.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IDProperty =
        DependencyProperty.Register("ID", typeof(int), typeof(FirstWindow), new PropertyMetadata(0));

    public FirstWindow()
    {
        InitializeComponent();
    }

    private void NextButton(object sender, RoutedEventArgs e)
    {
        try
        {
            BO.Worker? worker = bl.Worker.Read(ID);
            if (worker != null)
            {
                if (worker.Level == BO.WorkerExperience.Manager)
                    new MainWindow().ShowDialog();
                else
                    MessageBox.Show("We need to do it");
            }
        }
        catch (BlDoesNotExistsException ex)
        {
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
