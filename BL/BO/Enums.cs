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
}

public enum Status
{
    Unscheduled,
    Scheduled,
    OnTrack,
    Done,
    InJeopardy
}

public enum ProjectStatus
{
    Unscheduled,
    Scheduled,
    Execution
}

public enum PLWorkerExperience
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

public enum PLStatus
{
    Unscheduled,
    Scheduled,
    OnTrack,
    Done,
    All,
    InJeopardy
}