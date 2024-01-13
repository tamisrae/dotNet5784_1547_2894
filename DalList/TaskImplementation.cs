
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int id = DataSource.Config.NextTaskId;
        Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        Task? task = Read(id);
        if (task == null)
            throw new Exception($"Task with ID={id} doe's NOT exists");
        DataSource.Tasks.Remove(task);
    }

    public Task? Read(int id)
    {
        Task? task = DataSource.Tasks.Find(task => task.Id == id);
        if (task != null)
            return task;
        return null;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        if (Read(item.Id) == null)
            throw new Exception($"Task with ID={item.Id} doe's NOT exists");
        int id = item.Id;
        Delete(item.Id);
        Task task = new Task(item.Alias, item.Description, item.CreatedAtDate, item.IsMilestone, id, item.Complexity, item.WorkerId, item.RequiredEffortTime,
            item.StartDate, item.ScheduledDate, item.Deadlinedate, item.CompleteDate, item.Deliverables, item.Remarks);
        DataSource.Tasks.Add(task); 
    }
}
