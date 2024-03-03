
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static DataSource;


internal class UserImplementation : IUser
{
    public void Clear()
    {
        Users.Clear();
    }

    public int Create(User item)
    {
        foreach (User user in DataSource.Users)
        {
            if (user.Id == item.Id)
                throw new DalAlreadyExistsException($"User with ID={item.Id} already exists");
        }
        DataSource.Users.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        User? user = Read(id);
        if (user == null)
            return;
        DataSource.Users.Remove(user);
    }

    public User? ReadByPassword(string password)
    {
        return DataSource.Users.FirstOrDefault(user => user.Password == password);
    }

    public User? Read(Func<User, bool> filter) => Users.FirstOrDefault(filter);

    public User? Read(int id)
    {
        return DataSource.Users.FirstOrDefault(user => user.Id == id);
    }

    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Users
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Users
               select item;
    }

    public void Update(User item)
    {
        if (Read(item.Id) == null)
            throw new DalDoesNotExistsException($"User with ID={item.Id} doe's NOT exists");

        Delete(item.Id);
        Create(item);
    }
}

