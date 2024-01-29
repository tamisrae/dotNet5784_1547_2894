using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BlImplementation;
internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal dal = DalApi.Factory.Get;

    public int Create(BO.Task task)
    {
        DO.Task doTask = new DO.Task
           ( task.Alias,task.Description,task.CreatedAtDate,false,task.Id,task.Complexity,task.WorkerId,task.RequiredEffortTime,
           task.StartDate,task.ScheduledDate,DateTime.Now,task.CompleteDate,task.Deliverables,task.Remarks);
        try
        {
            int idTask = dal.Task.Create(doTask);
            return idTask;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Student with ID={task.Id} already exists", ex);
        }
        throw new NotImplementedException();
    }
    public void Delete(int id)
    {
        try
        {
            dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Student with ID={id} already exists", ex);
        }
    }

    public BO.Task? Read(int id)
    {
        throw new Exception($"You are tambal!");
    }

    public IEnumerable<TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        return from item in dal.Task.ReadAll()
               where filter!(Read(item.Id)!)
               select new BO.TaskInList
               {
                   Id = item.Id,
                   Alias = item.Alias,
                   Description = item.Description,
                   Status = item.GetStatus()
               };
    }


    public void Update(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public void UpdateTheStatus(int id, Status newStatus)
    {
        throw new NotImplementedException();
    }
}
