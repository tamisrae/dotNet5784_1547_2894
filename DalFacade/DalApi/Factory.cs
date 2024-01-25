using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static DalApi.Config;

namespace DalApi
{
    public static class Factory
    {
        public static IDal Get
        {
            get
            {
                string dalType = s_dalName ?? throw new DalConfigException($"DAL name is not extracted from the configuration");
                DalImplementation dal = s_dalPackages[dalType] ?? throw new DalConfigException($"Package for {dalType} is not found in packages list in dal-config.xml");

                try { Assembly.Load(dal.Package ?? throw new DalConfigException($"Package {dal.Package} is null")); }
                catch (Exception ex) { throw new DalConfigException($"Failed to load {dal.Package}.dll package", ex); }

                Type type = Type.GetType($"{dal.Namespace}.{dal.Class}, {dal.Package}") ??
                    throw new DalConfigException($"Class {dal.Namespace}.{dal.Class} was not found in {dal.Package}.dll");

                return type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as IDal ??
                    throw new DalConfigException($"Class {dal.Class} is not a singleton or wrong property name for Instance");
            }
        }
    }

}
