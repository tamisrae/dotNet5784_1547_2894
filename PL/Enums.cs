using System.Collections;

namespace PL
{
    internal class PLWorkerExperienceCollection : IEnumerable
    {
        static readonly IEnumerable<BO.PLWorkerExperience> s_enums =
        (Enum.GetValues(typeof(BO.PLWorkerExperience)) as IEnumerable<BO.PLWorkerExperience>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class WorkerExperienceCollection : IEnumerable
    {
        static readonly IEnumerable<BO.WorkerExperience> s_enums =
        (Enum.GetValues(typeof(BO.WorkerExperience)) as IEnumerable<BO.WorkerExperience>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class ProjectStatusCollection : IEnumerable
    {
        static readonly IEnumerable<BO.ProjectStatus> s_enums =
        (Enum.GetValues(typeof(BO.ProjectStatus)) as IEnumerable<BO.ProjectStatus>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class StatusCollection : IEnumerable
    {
        static readonly IEnumerable<BO.Status> s_enums =
        (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class PLStatusCollection : IEnumerable
    {
        static readonly IEnumerable<BO.PLStatus> s_enums =
        (Enum.GetValues(typeof(BO.PLStatus)) as IEnumerable<BO.PLStatus>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
}
