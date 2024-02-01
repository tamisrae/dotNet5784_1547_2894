
namespace Dal;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";

    /// <summary>
    /// This function clear all the data from the xml file
    /// </summary>
    public void Clear()
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        tasks.Clear();
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
    }

    /// <summary>
    /// This function create a new task
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        int id = Config.NextTaskId;
        //DO.Task copy = item with { Id = Config.NextTaskId };
        tasks.Add(item with { Id = id });
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
        return id;
    }

    /// <summary>
    /// This function delete task from the xml file
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        DO.Task? task = Read(id);
        if (task == null)
            throw new DalDoesNotExistsException($"Task with ID={id} doe's NOT exists");
        tasks.Remove(task);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
    }

    /// <summary>
    /// This function read task from the xml file by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    public DO.Task? Read(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        return tasks.FirstOrDefault(task => task.Id == id) ?? throw new DalDoesNotExistsException($"Task with ID={id} doe's NOT exists");
    }

    /// <summary>
    /// This function read task from the xml file by filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        return tasks.FirstOrDefault(filter);
    }

    /// <summary>
    /// This function read all the tasks that fit the filter from the xml file
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<DO.Task> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        if (filter != null)
        {
            return from item in tasks
                   where filter(item)
                   select item;
        }
        return from item in tasks
               select item;
    }

    /// <summary>
    /// This function update a task
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        if (Read(item.Id) == null)
            throw new DalDoesNotExistsException($"Task with ID={item.Id} doe's NOT exists");

        int id = item.Id;
        DO.Task taskToDelete = Read(id)!;
        tasks.Remove(taskToDelete);
        DO.Task task = new DO.Task(item.Alias, item.Description, item.CreatedAtDate, item.IsMilestone, id, item.Complexity, item.WorkerId, item.RequiredEffortTime,
            item.StartDate, item.ScheduledDate, item.Deadlinedate, item.CompleteDate, item.Deliverables, item.Remarks);
        tasks.Add(task);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
    }
}

