using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }

    internal static DateTime? GetProjectDate(string name)
    {
        XElement root = XMLTools.LoadListFromXMLElement(@"..\xml\ + s_data_config_xml");
        return DateTime.TryParse(root.Element(name)?.Value, out DateTime dateTime) ? dateTime : (DateTime?)null;
    }

    internal static void SetProjectDate(string name, DateTime? dateTime)
    {
        string path = @"..\xml\" + s_data_config_xml;
        XElement root = XMLTools.LoadListFromXMLElement(path);
        XElement elementToUpdate = root.Element(name)!;

        if (elementToUpdate != null)
        {
            elementToUpdate.ReplaceWith(new XElement(name, dateTime.ToString()));
            XMLTools.SaveListToXMLElement(root, path);
        }
    }
}
