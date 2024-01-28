using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ITaskInList
{
    public int Create(BO.TaskInList task);
    public BO.TaskInList? Read(int id);
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.TaskInList, bool>? filter = null);
    public void Update(BO.TaskInList task);
    public void Delete(int id);
    public void UpdateTheStatus(int id, Status newStatus);
}
