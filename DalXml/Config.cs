using System.Xml.Linq;

namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextTaskId
    {
        get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId");
        set => XMLTools.SetNextId(s_data_config_xml, "NextTaskId", value);
    }

    internal static int NextDependencyId
    {
        get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId");
        set => XMLTools.SetNextId(s_data_config_xml, "NextDependencyId", value);
    }

    internal static DateTime? GetProjectDate(string name)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_data_config_xml);
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
