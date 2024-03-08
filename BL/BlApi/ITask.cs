using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ITask
{
    public int Create(BO.Task task);
    public BO.Task? Read(int id);
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null);
    public void Update(BO.Task task);
    public void Delete(int id);
    public void Clear();
    public void UpdateTheScheduledDate(int taskId, DateTime scheduledDate);
    public IEnumerable<TaskInList>? TasksForWorker(int workerId);
    public void AutomaticSchedule();
    public void StartTask(BO.Task task, int workerId);
    public void EndTask(int taskId, int workerId);
    public void SignUpForTask(int taskId, int workerId);
    public IEnumerable<IGrouping<int?, BO.TaskInList>> GroupTasksByComplexity();
    public BO.TaskInList? ReadTaskInList(int id);
}
