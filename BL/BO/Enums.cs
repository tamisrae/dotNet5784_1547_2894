using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
public enum WorkerExperience
{
    Chef,
    SousChef,
    Waiter,
    Cleaner,
    Driver,
    Secretary,
    Manager,
    All
}

public enum Status
{
    Unscheduled,
    Scheduled,
    OnTrack,
    Done
}

public enum ProjectStatus
{
    Unscheduled,
    Scheduled,
    Execution
}