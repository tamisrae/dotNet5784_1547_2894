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

namespace PL.User;

/// <summary>
/// Interaction logic for UserListWindow.xaml
/// </summary>
public partial class UserListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public UserListWindow()
    {
        InitializeComponent();
        UserList = s_bl?.User.ReadAll()!;
    }



    public IEnumerable<BO.User> UserList
    {
        get { return (IEnumerable<BO.User>)GetValue(UserListProperty); }
        set { SetValue(UserListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for UserList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty UserListProperty =
        DependencyProperty.Register("UserList", typeof(IEnumerable<BO.User>), typeof(UserListWindow), new PropertyMetadata(null));

    private void ShowUser(object sender, MouseButtonEventArgs e)
    {
        BO.User? user = (sender as ListView)?.SelectedItem as BO.User;
        if (user != null)
        {
            new UserWindow(user.Id).ShowDialog();
            //update the list of the workers after the changes
            UserList = s_bl?.User.ReadAll()!;
        }
    }
}
