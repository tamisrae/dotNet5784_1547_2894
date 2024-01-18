
namespace Dal;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
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

    public int Create(Dependency item)
    {
        XElement? dpncRoot = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? element = dpncRoot.Elements().FirstOrDefault(dpnc => (int?)dpnc.Element("id") == item.Id);
        if (element != null)
            throw new DalAlreadyExistsException($"Worker with ID={item.Id} already exists");
        else
        {
            dpncRoot.Add(item);
            XMLTools.SaveListToXMLElement(dpncRoot, s_dependencies_xml);
            return item.Id;
        }
    }

    public void Delete(int id)
    {
        XElement? dpncRoot = XMLTools.LoadListFromXMLElement(s_dependencies_xml);

        XElement? element = dpncRoot.Elements().FirstOrDefault(dpnc => (int?)dpnc.Element("id") == id);
        if (element != null)
        {
            element.Remove();
            XMLTools.SaveListToXMLElement(dpncRoot, s_dependencies_xml);
        }
        else
            throw new DalDoesNotExistException($"Dependency with ID={id} doe's NOT exists");
    }

    public Dependency? Read(int id)
    {
        XElement? dpncRoot = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? element = dpncRoot.Elements().FirstOrDefault(dpnc => (int?)dpnc.Element("Id") == id);
        if (element != null)
            return getDependency(element);
        else
            return null;
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().Select(dpnc => getDependency(dpnc)).FirstOrDefault(filter);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().Select(dpnc => getDependency(dpnc));
        else
            return XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().Select(dpnc => getDependency(dpnc)).Where(filter);
    }

    public void Update(Dependency item)
    {
        XElement? dpncRoot = XMLTools.LoadListFromXMLElement(s_dependencies_xml);

        XElement? element = dpncRoot.Elements().FirstOrDefault(dpnc => (int?)dpnc.Element("id") == item.Id);
        if (element == null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} doe's NOT exists");
        else
        {
            int id = item.Id;
            DO.Dependency dpnc = new Dependency(id, item.DependentTask, item.DependsOnTask);
            Delete(item.Id);
            dpncRoot.Add(dpnc);
            XMLTools.SaveListToXMLElement(dpncRoot, s_dependencies_xml);
        }

    }
}

