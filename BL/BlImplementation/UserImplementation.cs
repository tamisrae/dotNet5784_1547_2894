using BlApi;
using BO;
using DO;

namespace BlImplementation;

internal class UserImplementation : IUser
{
    private DalApi.IDal dal = DalApi.Factory.Get;

    public void Clear()
    {
        dal.User.Clear();
    }

    public int Create(BO.User user)
    {
        if (user.UserName.IsEmptyString() || user.Password.IsEmptyString() || user.Id.IsGreaterThanZero())
            throw new BlWorngValueException("The user has WORNG VALUE!");

        DO.User doUser = new DO.User(user.Id, user.UserName, user.Password);
        try
        {
            int idUser = dal.User.Create(doUser);
            return idUser;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"User with ID={user.Id} already exists", ex);
        }
    }

    public BO.User? Read(int id)
    {
        try
        {
            DO.User? doUser = dal.User.Read(id);
            if (doUser == null)
                throw new BO.BlDoesNotExistsException($"User with ID= {id} doe's NOT exists");

            return new BO.User
            {
                Id = doUser.Id,
                UserName = doUser.UserName,
                Password = doUser.Password
            };
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"User with ID= {id} doe's NOT exists", ex);
        }
    }

    public IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter = null)
    {
        if (filter == null)
        {
            IEnumerable<BO.User> list = from item in dal.User.ReadAll()//without filter
                                        select new BO.User
                                        {
                                            Id = item.Id,
                                            UserName = item.UserName,
                                            Password = item.Password
                                        };
            return list;
        }
        else
        {
            IEnumerable<BO.User> list = from item in dal.User.ReadAll()//with filter
                                          where filter!(Read(item.Id)!)
                                          select new BO.User
                                          {
                                              Id = item.Id,
                                              UserName = item.UserName,
                                              Password = item.Password
                                          };
            return list;
        }
    }

    public BO.User? ReadByPassword(string password)
    {
        try
        {
            DO.User? doUser = dal.User.ReadByPassword(password);
            if (doUser == null)
                throw new BO.BlDoesNotExistsException($"User with Password={password} doe's NOT exists");

            return new BO.User
            {
                Id = doUser.Id,
                UserName = doUser.UserName,
                Password = doUser.Password
            };
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"User with Password={password} doe's NOT exists", ex);
        }
    }

    public void Update(BO.User user)
    {
        if (user.UserName.IsEmptyString() || user.Password.IsEmptyString() || user.Id.IsGreaterThanZero())
            throw new BlWorngValueException("The user has WORNG VALUE!");

        DO.User doUser = new DO.User(user.Id, user.UserName, user.Password);

        try
        {
            dal.User.Update(doUser);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"User with ID={user.Id} doe's NOT exists", ex);
        }
    }
}
