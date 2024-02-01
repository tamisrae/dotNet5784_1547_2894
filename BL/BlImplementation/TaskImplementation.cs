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
        if (task.Id.IsGreaterThanZero() || task.Alias.IsEmptyString())
            throw new BlWorngValueException("The task has WORNG VALUE!");

        int? temp = (int?)task.Complexity;
        DO.WorkerExperience? complexity = null;
        if (temp != null)
            complexity = (DO.WorkerExperience)temp;

        if (task.Dependencies != null)
        {
            foreach (BO.TaskInList item in task.Dependencies)
            {
                DO.Dependency dependency = new DO.Dependency(0, task.Id, item.Id);
                dal.Dependency.Create(dependency);
            }
        }

        DO.Task doTask = new DO.Task
           (task.Alias, task.Description, task.CreatedAtDate, false, task.Id, complexity, task.WorkOnTask?.Id, task.RequiredEffortTime,
           task.StartDate, task.ScheduledDate, task.DeadlineDate, task.CompleteDate, task.Deliverables, task.Remarks);
        try
        {
            int idTask = dal.Task.Create(doTask);
            return idTask;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Student with ID={task.Id} already exists", ex);
        }
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
                throw new BO.BlDoesNotExistsException($"Task with ID={id} doe's NOT exists");

            BO.WorkerInTask? workerInTask;
            if (doTask.WorkerId != null)
            {
                DO.Worker worker = dal.Worker.Read((int)doTask.WorkerId)!;
                workerInTask = new BO.WorkerInTask { Id = (int)doTask.WorkerId, Name = worker.Name };
            }
            else
                workerInTask = null;

            int? temp = null;
            BO.WorkerExperience? complexity = null;
            if (doTask.Complexity != null)
            {
                temp = (int)doTask.Complexity;
                complexity = (BO.WorkerExperience)temp;
            }

            return new BO.Task
            {
                Id = id,
                Alias = doTask.Alias,
                Description = doTask.Description,
                Status = doTask.GetStatus(),
                WorkOnTask = workerInTask,
                Dependencies = null,
                CreatedAtDate = doTask.CreatedAtDate,
                ScheduledDate = doTask.ScheduledDate,
                StartDate = doTask.StartDate,
                CompleteDate = doTask.CompleteDate,
                ForeCastDate = doTask.GetForeCastDate(),
                DeadlineDate = doTask.Deadlinedate,
                RequiredEffortTime = doTask.RequiredEffortTime,
                Deliverables = doTask.Deliverables,
                Remarks = doTask.Remarks,
                Complexity = complexity
            };
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Task with ID={id} doe's NOT exists", ex);
        }
    }

    public IEnumerable<TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if (filter == null)
        {
            return from item in dal.Task.ReadAll()
                   select new BO.TaskInList
                   {
                       Id = item.Id,
                       Alias = item.Alias,
                       Description = item.Description,
                       Status = item.GetStatus()
                   };
        }
        else
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
    }

    public void Update(BO.Task task)
    {
        if (task.Id.IsGreaterThanZero() || task.Alias.IsEmptyString())
            throw new BlWorngValueException("The task has WORNG VALUE!");

        if (task.Dependencies != null)
        {
            List<DO.Dependency>? list = FindDependencies(task.Id);
            if (GetDependency(task.Id, list))
            {
                foreach (BO.TaskInList item in task.Dependencies)
                {
                    foreach (DO.Dependency depenc in dal.Dependency.ReadAll())
                    {
                        if (depenc.DependentTask != task.Id || depenc.DependsOnTask != item.Id)
                        {
                            DO.Dependency dependency = new DO.Dependency(0, task.Id, item.Id);
                            dal.Dependency.Create(dependency);
                        }
                    }
                }
            }
            else
                throw new BlCantUpdateException("This thask cannot be update!");
        }

        int? temp = (int?)task.Complexity;
        DO.WorkerExperience? complexity = null;
        if (temp != null)
            complexity = (DO.WorkerExperience)temp;

        int? workerId = null;
        if (task.WorkOnTask != null)
            workerId = task.WorkOnTask.Id;

        DO.Task doTask = new DO.Task(task.Alias, task.Description, task.CreatedAtDate, false, task.Id, complexity, workerId, task.RequiredEffortTime,
                                   task.StartDate, task.ScheduledDate, task.DeadlineDate, task.CompleteDate, task.Deliverables, task.Remarks);

        try
        {
            dal.Task.Update(doTask);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Worker with ID={task.Id} doe's NOT exists", ex);
        }
    }


    private List<DO.Dependency>? FindDependencies(int id)
    {
        List<DO.Dependency>? list = (from dependency in dal.Dependency.ReadAll()
                                     where dependency.DependentTask == id
                                     select dependency).ToList();
        return list;
    }


    private bool GetDependency(int taskId, List<DO.Dependency>? list)//not correct!!!!
    {
        if (list == null)
            return true;
        else
        {
            foreach (DO.Dependency dependency in list)
            {
                if (dependency.DependentTask == taskId)
                    return false;
                else
                    return GetDependency(taskId, FindDependencies(dependency.Id));
            }
        }
        return true;
    }


    //private bool GetDependency(int taskId, List<DO.Dependency>? list)//not correct!!!!
    //{
    //    if (list == null)
    //        return true;
    //    else
    //    {
    //        foreach (DO.Dependency dependency in list)
    //        {
    //            if (dependency.DependentTask == taskId)
    //                return false;
    //            else
    //                if (false == GetDependency(taskId, FindDependencies(dependency.Id)))
    //                    return false;


    //        }
    //        return true;
    //    }
    //    //return true;
    //}



}

/*
     string Alias,
    string Description,
    DateTime CreatedAtDate,
    bool IsMilestone,
    int Id,
    DO.WorkerExperience? Complexity = null,
    int? WorkerId = null,
    TimeSpan? RequiredEffortTime = null,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? Deadlinedate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null
 */