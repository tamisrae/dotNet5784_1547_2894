using System;
using System.Collections.Generic;
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
        foreach (PropertyInfo item in obj.GetType().GetProperties())
            str += "\n" + item.Name + ": " + item.GetValue(obj, null);
        return str;
    }

    public static string ToStringPropertyArray<T>(this T[] obj)
    {
        string str = "";
        foreach (T element in obj)
        {
            foreach (PropertyInfo item in obj.GetType().GetProperties())
                str += "\n" + item.Name + ": " + item.GetValue(obj, null);
        }
        return str;
    }

}
