using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IUser
{
    public int Create(BO.User user);
    public BO.User? ReadByPassword(string password);
    public BO.User? Read(int id);
    public IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter = null);
    public void Update(BO.User user);
    public void Clear();
}
