using PL.Task;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PL;

class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertIdToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertDateToBool : IValueConverter
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.ProjectStatus)value != BO.ProjectStatus.Unscheduled ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertLevelToVisibaleManager : IValueConverter
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.WorkerExperience)value == BO.WorkerExperience.Manager ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertLevelToVisibaleWorker : IValueConverter
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.WorkerExperience)value == BO.WorkerExperience.Manager ? Visibility.Hidden : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertStatusToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.Status)value == BO.Status.OnTrack || (BO.Status)value == BO.Status.Done ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertDependencyToContent : IValueConverter
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();

    //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //{
    //    object o = TaskWindow.DependenciesListProperty;

    //}
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //if (value is DependencyObject dependencyObject)
        //{
        //DependencyObject dependencyObject = (DependencyObject)TaskWindow.DependenciesListProperty.;
        //Object obj = dependencyObject.GetValue(TaskWindow.DependenciesListProperty);
        //if (obj != null && obj.GetType() == typeof(List<>))
        //{
        //    return ((List<BO.TaskInList>)obj).FirstOrDefault(item => item.Id == (int)value) == null ? "ADD" : "Delete";
        //}
        //}

        return null;
        //item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(List<>)
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}