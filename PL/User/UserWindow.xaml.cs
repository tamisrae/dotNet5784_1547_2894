using BO;
using System.Windows;

namespace PL.User;

/// <summary>
/// Interaction logic for UserWindow.xaml
/// </summary>
public partial class UserWindow : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();
    int ID;

    public UserWindow(int Id = 0)
    {
        InitializeComponent();
        ID = Id;
        if (Id == 0)//create new user window with default values
            CurrentUser = new BO.User { Id = 0, UserName = "", Password = "" };
        else//create new worker window with the user's data
        {
            try
            {
                CurrentUser = bl.User.Read(Id)!;
            }
            catch (BlDoesNotExistsException mess)
            {
                CurrentUser = null!;
                MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }

    public BO.User CurrentUser
    {
        get { return (BO.User)GetValue(CurrentUserProperty); }
        set { SetValue(CurrentUserProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentUserProperty =
        DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(UserWindow), new PropertyMetadata(null));

    private void AddUpdateUser(object sender, RoutedEventArgs e)
    {
        this.Close();
        if (CurrentUser != null)
        {
            try
            {
                if (ID != 0)//if the id is not 0 it means that we need to update the data
                {
                    bl.User.Update(CurrentUser);
                    MessageBox.Show("The user was successfully updated", "UPDATE", MessageBoxButton.OK);
                }
                else//if the id is 0 it means that we need to creat new user
                {
                    bl.User.Create(CurrentUser);
                    MessageBox.Show("The user was successfully added", "ADD", MessageBoxButton.OK);
                }
            }
            catch (BlDoesNotExistsException mess)
            {
                MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (Exception mess)
            {
                MessageBox.Show(mess.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }
}
