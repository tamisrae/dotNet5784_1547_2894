using BO;
using PL.User;
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



    public BO.User CurrentUser
    {
        get { return (BO.User)GetValue(CurrentUserProperty); }
        set { SetValue(CurrentUserProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentUserProperty =
        DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(FirstWindow), new PropertyMetadata(null));

    public FirstWindow()
    {
        InitializeComponent();
    }

    private void LogInButton(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        Grid grid = (Grid)button.Parent;

        TextBox userNameTextBox = (TextBox)grid.Children[1];
        TextBox passwordTextBox = (TextBox)grid.Children[3];

        string userName = (string)userNameTextBox.Text;
        string password = (string)passwordTextBox.Text;

        try
        {
            BO.User? user = bl.User.ReadByPassword(password);
            if (user != null)
            {
                if (userName != user.UserName)
                    MessageBox.Show("User name is not correct", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    new MainWindow(user.Id).ShowDialog();
            }
        }
        catch (Exception)
        {
            MessageBox.Show("Password is not correct", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void SignInButton(object sender, RoutedEventArgs e)
    {
        new UserWindow().ShowDialog();
    }
}
