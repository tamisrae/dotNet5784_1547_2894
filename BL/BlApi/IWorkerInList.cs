using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IWorkerInList
{
    public int Create(BO.WorkerInList worker);
    public BO.WorkerInList? Read(int id);
    public IEnumerable<BO.WorkerInList> ReadAll(Func<BO.WorkerInList, bool>? filter = null);
    public void Update(BO.WorkerInList worker);
    public void Delete(int id);
    public BO.WorkerInList GetDetailedTaskForWorker(int WorkerId, int TaskId);
}
