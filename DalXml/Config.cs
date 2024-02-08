using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal static class Config
    {
        static string s_data_config_xml = "data-config";
        internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
        internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }

        //internal static DateTime? StartProjectDate { get => XMLTools.GetDateTime(s_data_config_xml, "StartProjectDate"); }
    }
}
