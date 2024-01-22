
namespace Dal;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";


    static Dependency getDependency(XElement dpnc)
    {
        return new Dependency()
        {
            Id = int.TryParse((string?)dpnc.Element("Id"), out var id) ? id : throw new FormatException("can't convert id"),
            DependentTask = int.TryParse((string?)dpnc.Element("DependentTask"), out var dependentTask) ? dependentTask : throw new FormatException("can't convert Dependent Task"),
            DependsOnTask = int.TryParse((string?)dpnc.Element("DependsOnTask"), out var dependsOnTask) ? dependsOnTask : throw new FormatException("can't convert Depends On Task")
        };
    }

    /// <summary>
    /// This function clear all the data from the xml file
    /// </summary>
    public void Clear()
    {
        XElement? dpncRoot = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        dpncRoot.RemoveAll();
        XMLTools.SaveListToXMLElement(dpncRoot, s_dependencies_xml);
    }

    /// <summary>
    /// This function create a new dependency
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        XElement? dpncRoot = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? element = new XElement("dependencies");

        element.Add(new XElement("Id", Config.NextDependencyId));
        element.Add(new XElement("DependentTask", item.DependentTask));
        element.Add(new XElement("DependsOnTask", item.DependsOnTask));

        dpncRoot.Add(element);
        XMLTools.SaveListToXMLElement(dpncRoot, s_dependencies_xml);
        return int.Parse(element.Element("Id")!.Value);
    }

    /// <summary>
    /// This function delete dependency from the xml file
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        XElement? dpncRoot = XMLTools.LoadListFromXMLElement(s_dependencies_xml);

        XElement? element = dpncRoot.Elements().FirstOrDefault(dpnc => (int?)dpnc.Element("Id") == id);
        if (element != null)
        {
            element.Remove();
            XMLTools.SaveListToXMLElement(dpncRoot, s_dependencies_xml);
        }
        else
            throw new DalDoesNotExistException($"Dependency with ID={id} doe's NOT exists");
    }

    /// <summary>
    /// This function read dependency from the xml file by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        XElement? dpncRoot = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? element = dpncRoot.Elements().FirstOrDefault(dpnc => (int?)dpnc.Element("Id") == id);
        if (element != null)
            return getDependency(element);
        else
            throw new DalDoesNotExistException($"Dependency with ID={id} doe's NOT exists");
    }

    /// <summary>
    /// This function read dependency from the xml file by filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().Select(dpnc => getDependency(dpnc)).FirstOrDefault(filter);
    }

    /// <summary>
    /// This function read all the dependencies that fit the filter from the xml file
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().Select(dpnc => getDependency(dpnc));
        else
            return XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().Select(dpnc => getDependency(dpnc)).Where(filter);
    }

    /// <summary>
    /// This function update a dependency
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependency item)
    {
        XElement? dpncRoot = XMLTools.LoadListFromXMLElement(s_dependencies_xml);

        XElement? element = dpncRoot.Elements().FirstOrDefault(dpnc => (int?)dpnc.Element("Id") == item.Id);
        if (element != null)
        {
            element!.Element("DependentTask")!.Value = Convert.ToString(item.DependentTask);
            element!.Element("DependsOnTask")!.Value = Convert.ToString(item.DependsOnTask);

            XMLTools.SaveListToXMLElement(dpncRoot, s_dependencies_xml);
        }
        else
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} doe's NOT exists");

    }
}

