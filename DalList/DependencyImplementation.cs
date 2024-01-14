
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using static DataSource;


internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        DataSource.Dependencies.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        Dependency? dependency = Read(id);
        if (dependency == null)
            throw new DalDoesNotExistException($"Dependency with ID={id} doe's NOT exists");
        DataSource.Dependencies.Remove(dependency);
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(dependency => dependency.Id == id);

        //Dependency? dependency = DataSource.Dependencys.Find(dependency => dependency.Id == id); 
        //if (dependency != null)
        //    return dependency;
        //return null;
    }

    //public List<Dependency> ReadAll()
    //{
    //    return new List<Dependency>(DataSource.Dependencys);
    //}

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Dependencies
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependencies
               select item;
    }

    public void Update(Dependency item)
    {
        if (Read(item.Id) == null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} doe's NOT exists");
        int id = item.Id;
        Delete(item.Id);
        Dependency dependency = new Dependency(id, item.DependentTask, item.DependsOnTask);
        DataSource.Dependencies.Add(dependency);
    }


    public Dependency? Read(Func<Dependency, bool> filter) => Dependencies.FirstOrDefault(filter);

}
