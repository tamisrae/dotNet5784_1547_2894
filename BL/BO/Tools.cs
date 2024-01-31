using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BO;

static class Tools
{
    public static string ToStringProperty<T>(this T obj)
    {
        string str = "";
        foreach (PropertyInfo item in obj?.GetType().GetProperties()!)
        {
            if (obj is IEnumerable<T> && !(obj is string))
            {
                foreach (T element in (IEnumerable<T>)obj)
                    str += "\n" + item.Name + ": " + item.GetValue(obj, null);
            }
            else
                str += "\n" + item.Name + ": " + item.GetValue(obj, null);
        }
        return str;
    }

    //public static string ToStringPropertyIEnumerable<T>(this IEnumerable<T> obj)
    //{
    //    string str = "";
    //    foreach (T element in obj)
    //        str = element.ToStringProperty();
    //    return str;
    //}

    public static bool IsGreaterThanZero<T>(this T value) where T : IComparable<T>
    {
        return value.CompareTo(default(T)) > 0;
    }

    public static bool IsEmptyString(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    public static Status GetStatus(this DO.Task task)
    {
        Status status;
        if (task.ScheduledDate == null)
            status = Status.Unscheduled;
        else if (task.ScheduledDate != null && (task.StartDate == null || task.StartDate > DateTime.Now))
            status = Status.Scheduled;
        else if (task.StartDate != null && task.StartDate < DateTime.Now && (task.CompleteDate == null || task.CompleteDate > DateTime.Now))
            status = Status.OnTrack;
        else
            status = Status.Done;

        return status;
    }

    public static DateTime? GetForeCastDate(this DO.Task task)
    {
        DateTime? startDate;
        if (task.StartDate > task.ScheduledDate)
            startDate = task.StartDate;
        else
            startDate = task.ScheduledDate;

        DateTime? foreCastDate = startDate + task.RequiredEffortTime;
        return foreCastDate;
    }
}

