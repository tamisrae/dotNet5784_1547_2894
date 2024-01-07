
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
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
        Task? task = DataSource.Tasks.Find(task => task.Id == id);
        if (task == null)
            throw new Exception($"Task with ID={id} doe's NOT exists");
        DataSource.Tasks.Remove(new Task { Id = id });
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
        Delete(item.Id);
        Create(item);
    }
}
