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

class ConvertBoolToVisibale : IValueConverter
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertLevelToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.WorkerExperience)value == BO.WorkerExperience.Manager ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConvertLevelAndDateToBool : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        BO.WorkerExperience level = (BO.WorkerExperience)values[0];
        BO.ProjectStatus projectStatus = (BO.ProjectStatus)values[1];
        if (level == BO.WorkerExperience.Manager && projectStatus == BO.ProjectStatus.Unscheduled)
            return true;
        else
            return false;
    }

    public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

//class ConvertDep : IValueConverter
//{
//    static readonly BlApi.IBl bl = BlApi.Factory.Get();

//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        int Id = (int)value;
//        if (parameter != null)
//        {
//            List<BO.TaskInList> depenndencies = (List<BO.TaskInList>)parameter;

//            if (depenndencies.FirstOrDefault(item => item.Id == Id) == null)
//                return "Add";
//            else
//                return "Delete";
//        }
//        else
//            return "Add";
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }
//}

public class ConvertStatusAndLevelToBool : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        BO.WorkerExperience level = (BO.WorkerExperience)values[0];
        BO.Status status = (BO.Status)values[1];
        if (level == BO.WorkerExperience.Manager && status == BO.Status.Scheduled)
            return true;
        else
            return false;
    }

    public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertStatusToBoolStartTask : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.Status)value == BO.Status.Scheduled ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertStatusToBoolEndTask : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.Status)value == BO.Status.OnTrack ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}