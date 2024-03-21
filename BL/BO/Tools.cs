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
}

