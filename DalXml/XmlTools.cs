namespace Dal;

using DO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

static class XMLTools
{
    const string s_xml_dir = @"..\xml\";
    static XMLTools()
    {
        if (!Directory.Exists(s_xml_dir))
            Directory.CreateDirectory(s_xml_dir);
    }

    #region Extension Fuctions
    public static T? ToEnumNullable<T>(this XElement element, string name) where T : struct, Enum =>
        Enum.TryParse<T>((string?)element.Element(name), out var result) ? (T?)result : null;
    public static DateTime? ToDateTimeNullable(this XElement element, string name) =>
        DateTime.TryParse((string?)element.Element(name), out var result) ? (DateTime?)result : null;
    public static double? ToDoubleNullable(this XElement element, string name) =>
        double.TryParse((string?)element.Element(name), out var result) ? (double?)result : null;
    public static int? ToIntNullable(this XElement element, string name) =>
        int.TryParse((string?)element.Element(name), out var result) ? (int?)result : null;
    #endregion

    #region XmlConfig
    public static int GetAndIncreaseNextId(string data_config_xml, string elemName)
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config_xml);
        int nextId = root.ToIntNullable(elemName) ?? throw new FormatException($"can't convert id.  {data_config_xml}, {elemName}");
        root.Element(elemName)?.SetValue((nextId + 1).ToString());
        XMLTools.SaveListToXMLElement(root, data_config_xml);
        return nextId;
    }
    #endregion

    #region SaveLoadWithXElement
    public static void SaveListToXMLElement(XElement rootElem, string entity)
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            rootElem.Save(filePath);
        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to create xml file: {s_xml_dir + filePath}, {ex.Message}");
        }
    }
    public static XElement LoadListFromXMLElement(string entity)
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            if (File.Exists(filePath))
                return XElement.Load(filePath);
            XElement rootElem = new(entity);
            rootElem.Save(filePath);
            return rootElem;
        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to load xml file: {s_xml_dir + filePath}, {ex.Message}");
        }
    }
    #endregion

    #region SaveLoadWithXMLSerializer
    public static void SaveListToXMLSerializer<T>(List<T> list, string entity) where T : class
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            using FileStream file = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            new XmlSerializer(typeof(List<T>)).Serialize(file, list);
        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to create xml file: {s_xml_dir + filePath}, {ex.Message}");
        }
    }
    public static List<T> LoadListFromXMLSerializer<T>(string entity) where T : class
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            if (!File.Exists(filePath)) return new();
            using FileStream file = new(filePath, FileMode.Open);
            XmlSerializer x = new(typeof(List<T>));
            return x.Deserialize(file) as List<T> ?? new();
        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to load xml file: {filePath}, {ex.Message}");
        }
    }
    #endregion
}


