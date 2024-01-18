
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

    public int Create(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        int id = Config.NextTaskId;
        DO.Task copy = item with { Id = id };
        tasks.Add(copy);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
        return id;
    }

    public void Delete(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        DO.Task? task = Read(id);
        if (task == null)
            throw new DalDoesNotExistException($"Task with ID={id} doe's NOT exists");
        tasks.Remove(task);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
    }

    public DO.Task? Read(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        return tasks.FirstOrDefault(task => task.Id == id) ?? throw new DalDoesNotExistException($"Task with ID={id} doe's NOT exists");
    }

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        return tasks.FirstOrDefault(filter);
    }

    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
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

    public void Update(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        if (Read(item.Id) == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} doe's NOT exists");
        int id = item.Id;
        Delete(item.Id);
        DO.Task task = new DO.Task(item.Alias, item.Description, item.CreatedAtDate, item.IsMilestone, id, item.Complexity, item.WorkerId, item.RequiredEffortTime,
            item.StartDate, item.ScheduledDate, item.Deadlinedate, item.CompleteDate, item.Deliverables, item.Remarks);
        tasks.Add(task);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
    }
}

