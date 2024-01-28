
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using static DataSource;

internal class TaskImplementation : ITask
{
    /// <summary>
    /// Craete a new task
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Task item)
    {
        int id = DataSource.Config.NextTaskId;
        Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }

    /// <summary>
    /// Delete a task from the list
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Task? task = Read(id);
        if (task == null)
            throw new DalDoesNotExistsException($"Task with ID={id} doe's NOT exists");
        DataSource.Tasks.Remove(task);
    }

    /// <summary>
    /// Read a task form the list by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(task => task.Id == id) ?? throw new DalDoesNotExistsException($"Task with ID={id} doe's NOT exists");
    }

    /// <summary>
    /// Read all the tasks that meet the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }

    /// <summary>
    /// Update a task by ID
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Task item)
    {
        if (Read(item.Id) == null)
            throw new DalDoesNotExistsException($"Task with ID={item.Id} doe's NOT exists");
        int id = item.Id;
        Delete(item.Id);
        Task task = new Task(item.Alias, item.Description, item.CreatedAtDate, item.IsMilestone, id, item.Complexity, item.WorkerId, item.RequiredEffortTime,
            item.StartDate, item.ScheduledDate, item.Deadlinedate, item.CompleteDate, item.Deliverables, item.Remarks);
        DataSource.Tasks.Add(task);
    }

    /// <summary>
    /// Read a task that meets the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task? Read(Func<Task, bool> filter) => Tasks.FirstOrDefault(filter);

    public void Clear()
    {
        Tasks.Clear();
    }
}
