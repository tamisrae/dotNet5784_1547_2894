
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using static DataSource;

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
            throw new DalDoesNotExistException($"Task with ID={id} doe's NOT exists");
        DataSource.Tasks.Remove(task);
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(task => task.Id == id);
        //Task? task = DataSource.Tasks.Find(task => task.Id == id);
        //if (task != null)
        //    return task;
        //return null;
    }

    //public List<Task> ReadAll()
    //{
    //    return new List<Task>(DataSource.Tasks);
    //}

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

    public void Update(Task item)
    {
        if (Read(item.Id) == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} doe's NOT exists");
        int id = item.Id;
        Delete(item.Id);
        Task task = new Task(item.Alias, item.Description, item.CreatedAtDate, item.IsMilestone, id, item.Complexity, item.WorkerId, item.RequiredEffortTime,
            item.StartDate, item.ScheduledDate, item.Deadlinedate, item.CompleteDate, item.Deliverables, item.Remarks);
        DataSource.Tasks.Add(task); 
    }


    public Task? Read(Func<Task, bool> filter) => Tasks.FirstOrDefault(filter);
}
