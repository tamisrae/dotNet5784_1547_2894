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
        try
        {
            DO.Task? doTask = dal.Task.Read(id);
            if (doTask == null)
                throw new BO.BlDoesNotExistsException($"Student with ID={id} already exists");

            return new BO.Task()
            {
               Id=id,
               Alias=doTask.Alias,
               Description=doTask.Description,
               Status=
            };
        }
            /*
           public int Id { get; init; }
    public required string Alias { get; set; }
    public required string Description { get; set; }
    public Status Status { get; set; }
    public TaskInList? DependencyList { get; set; } = null;
    public DateTime CreatedAtDate {  get; set; }
    public DateTime? ScheduledDate { get; set; } = null;
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EstimatedCompletionDate { get; set; } = null;
    public DateTime? CompleteDate { get; set; } = null;
    public TimeSpan? RequiredEffortTime { get; set; } = null;
    public string? Deliverables { get; set; } = null;
    public string? Remarks { get; set; } = null;
    public DO.WorkerExperience? Complexity { get; set; } = null;
    public int? WorkerId { get; set; } = null;
     */

    public IEnumerable<TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
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
