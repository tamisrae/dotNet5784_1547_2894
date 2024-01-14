
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using static DataSource;


internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// Create a new dependency
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        DataSource.Dependencies.Add(copy);
        return id;
    }

    /// <summary>
    /// Delete a dependency from the list
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Dependency? dependency = Read(id);
        if (dependency == null)
            throw new DalDoesNotExistException($"Dependency with ID={id} doe's NOT exists");
        DataSource.Dependencies.Remove(dependency);
    }

    /// <summary>
    /// Read a dependency form the list by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(dependency => dependency.Id == id) ?? throw new DalDoesNotExistException($"Dependency with ID={id} doe's NOT exists");
    }

    /// <summary>
    /// Read all the dependencies that meet the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Update a dependency by ID
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependency item)
    {
        if (Read(item.Id) == null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} doe's NOT exists");
        int id = item.Id;
        Delete(item.Id);
        Dependency dependency = new Dependency(id, item.DependentTask, item.DependsOnTask);
        DataSource.Dependencies.Add(dependency);
    }

    /// <summary>
    /// Read a dependency that meets the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Dependency? Read(Func<Dependency, bool> filter) => Dependencies.FirstOrDefault(filter);
}
