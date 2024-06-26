﻿using BO;
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



    public bool ShowPassword
    {
        get { return (bool)GetValue(ShowPasswordProperty); }
        set { SetValue(ShowPasswordProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ShowPassword.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShowPasswordProperty =
        DependencyProperty.Register("ShowPassword", typeof(bool), typeof(FirstWindow), new PropertyMetadata(true));




    public FirstWindow()
    {
        try
        {
            if (bl.Worker.Read(123) == null)
            {
            }
        }
        catch (BlDoesNotExistsException)
        {
            CurrentUser = new BO.User { Id = 0, UserName = "", Password = "" };
            Worker manager = new BO.Worker { Id = 123, Level = BO.WorkerExperience.Manager, Email = "shir@gmail.com", Cost = 250, Name = "Shir" };
            bl!.Worker.Create(manager);
            BO.User managerUser = new BO.User { Id = 123, UserName = "shir", Password = "123" };
            bl!.User.Create(managerUser);
        }
        CurrentUser = new BO.User { Id = 0, UserName = "", Password = "" };
        InitializeComponent();
    }

    private void LogInButton(object sender, RoutedEventArgs e)
    {
        try
        {
            BO.User? user = bl.User.ReadByPassword(CurrentUser.Password);
            if (user != null)
            {
                if (CurrentUser.UserName != user.UserName)
                    MessageBox.Show("User name is not correct", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    new MainWindow(user.Id).Show();
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

    private void PasswordBox_Click(object sender, RoutedEventArgs e)
    {
        PasswordBox passwordBox = (sender as PasswordBox)!;
        if (passwordBox != null)
            CurrentUser.Password = passwordBox.Password;
    }
}
