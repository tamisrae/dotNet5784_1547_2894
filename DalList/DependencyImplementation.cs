
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        DataSource.Dependencys.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        Dependency? dependency = Read(id);
        if (dependency == null)
            throw new Exception($"Dependency with ID={id} doe's NOT exists");
        DataSource.Dependencys.Remove(dependency);
    }

    public Dependency? Read(int id)
    {
        Dependency? dependency = DataSource.Dependencys.Find(dependency => dependency.Id == id);
        if (dependency != null)
            return dependency;
        return null;
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencys);
    }

    public void Update(Dependency item)
    {
        if (Read(item.Id) == null)
            throw new Exception($"Dependency with ID={item.Id} doe's NOT exists");
        int id = item.Id;
        Delete(item.Id);
        Dependency dependency = new Dependency(id, item.DependentTask, item.DependsOnTask);
        DataSource.Dependencys.Add(dependency);
    }
}
