using System.Collections;

namespace PL
{

    internal class Enums
    {

    }
    internal class WorkerExperienceCollection : IEnumerable
    {
        static readonly IEnumerable<BO.WorkerExperience> s_enums =
        (Enum.GetValues(typeof(BO.WorkerExperience)) as IEnumerable<BO.WorkerExperience>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
}
