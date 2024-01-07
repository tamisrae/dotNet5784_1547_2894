﻿
namespace Dal;
 static internal class DataSource
{
    internal static class Config
    {
        internal const int taskId = 1;
        private static int nextTaskId = taskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal const int dependencyId = 1000;
        private static int nextDependencyId = dependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
    }
    internal static List<DO.Worker> Workers { get; } = new();
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Dependency> Dependencys { get; } = new();

}