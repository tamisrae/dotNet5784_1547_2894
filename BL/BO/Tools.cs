using DalApi;
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
    /// <summary>
    /// A function that examines the entity at runtime and prints
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToStringProperty<T>(this T obj)
    {
        string str = "";
        foreach (PropertyInfo item in typeof(T).GetProperties()!)
        {
            if (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                if (item.GetValue(obj, null) != null)
                {
                    var value = item.GetValue(obj) as IEnumerable<object>;
                    if (value != null)
                    {
                        str += $"{item.Name}:\n";
                        foreach (var i in value)
                            str += $"{i}";
                        if (str.EndsWith(" "))
                            str.Remove(str.Length - 1);
                        str += '\n';
                    }
                }
            }
            else if (item.GetValue(obj, null) != null)
                str += $"{item.Name}: {item.GetValue(obj)}\n";
        }
        return str;
    }

    /// <summary>
    /// A function that checks if T is less than zero
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsGreaterThanZero<T>(this T value) where T : IComparable<T>
    {
        return value.CompareTo(default(T)) < 0;
    }

    /// <summary>
    /// A function that checks if string is empty
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsEmptyString(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// A function that returns the status of task
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
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

    /// <summary>
    /// A function that calculate and returns the fore cast date
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    public static DateTime? GetForeCastDate(this DO.Task task)
    {
        DateTime? startDate;
        if (task.StartDate != null && task.StartDate > task.ScheduledDate)
            startDate = task.StartDate;
        else
            startDate = task.ScheduledDate;

        DateTime? foreCastDate = startDate + task.RequiredEffortTime;
        return foreCastDate;
    }


    private static DalApi.IDal dal = DalApi.Factory.Get;
    /// <summary>
    /// A function that checks if worker can take a task
    /// </summary>
    /// <param name="workerId"></param>
    /// <param name="task"></param>
    /// <returns></returns>
    public static bool AllowedTask(int workerId, DO.Task task)
    {
        if (task.WorkerId != null && workerId != task.WorkerId)//if another worker work on this task
            return false;
        DO.Worker? worker = dal.Worker.Read(workerId);
        if (worker != null && worker.Level != task.Complexity)//if the level of the worker does not fit to the complexity of the task
            return false;

        IEnumerable<DO.Task>? tasks = (from dependency in dal.Dependency.ReadAll()
                                              where dependency.DependentTask == task.Id
                                              select dal.Task.Read(dependency.DependsOnTask));

        DO.Task? tempTask = tasks.FirstOrDefault(t => t.GetStatus() != BO.Status.Done);//If one of the tasks that the task depends on has not finished
        if (tempTask != null)
            return false;
        return true;
    }
}

