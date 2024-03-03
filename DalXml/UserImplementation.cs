namespace Dal;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

internal class UserImplementation : IUser
{
    readonly string s_users_xml = "users";

    public void Clear()
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        users.Clear();
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_users_xml);
    }

    public int Create(User item)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        foreach (User user in users)
        {
            if (user.Id == item.Id)
                throw new DalAlreadyExistsException($"User with ID={item.Id} already exists");
        }
        users.Add(item);
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_users_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        User? user = Read(id);
        if (user == null)
            return;
        users.Remove(user);
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_users_xml);
    }

    public User? Read(Func<User, bool> filter)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);

        return users.FirstOrDefault(filter);
    }

    public User? Read(int id)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);

        return users.FirstOrDefault(user => user.Id == id);
    }

    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);

        if (filter != null)
        {
            return from item in users
                   where filter(item)
                   select item;
        }
        return from item in users
               select item;
    }

    public void Update(User item)
    {
        if (Read(item.Id) == null)
            throw new DalDoesNotExistsException($"User with ID={item.Id} doe's NOT exists");
        Delete(item.Id);
        Create(item);
    }

    public User? ReadByPassword(string password)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);

        return users.FirstOrDefault(user => user.Password == password);
    }
}
