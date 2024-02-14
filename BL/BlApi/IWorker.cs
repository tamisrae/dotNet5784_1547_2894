using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IWorker
{
    public int Create(BO.Worker worker);
    public BO.Worker? Read(int id);
    public IEnumerable<BO.Worker> ReadAll(Func<BO.Worker, bool>? filter = null);
    public void Update(BO.Worker worker);
    public void Delete(int id);
    public void Clear();
}
