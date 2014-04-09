using System;
using System.Collections.Generic;
using System.Linq;
using AgUnit.Runner.Resharper80.UnitTestFramework.Silverlight;
using AgUnit.Runner.Resharper80.Util;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper80.UnitTestFramework.SilverlightPlatform
{
    public static class UnitTestRunExtensions
    {
        public static bool IsSilverlightRun(this IUnitTestRun run)
        {
            return run.GetSilverlightPlatformVersion() != null;
        }

        public static Version GetSilverlightPlatformVersion(this IUnitTestRun run)
        {
            var sequence = run.GetRootTasks().FirstOrDefault();
            return sequence != null ? sequence.GetSilverlightPlatformVersion() : null;
        }

        public static IList<RemoteTaskPacket> GetRootTasks(this IUnitTestRun run)
        {
            return (run.GetTasks() as IList<RemoteTaskPacket>) ?? new List<RemoteTaskPacket>();
        }

        public static IEnumerable<RemoteTaskPacket> GetAllTasks(this IUnitTestRun run)
        {
            return run.GetTasks().SelectMany(t => t.GetAllTasksRecursive());
        }

        public static void AddTaskSequence(this IUnitTestRun run, RemoteTaskPacket sequence, SilverlightUnitTestElement silverlightElement, IUnitTestRun originalRun)
        {
            var runTasks = run.GetField<Dictionary<RemoteTask, IUnitTestElement>>("myTasks");
            var runTaskIdsToElements = run.GetField<Dictionary<string, IUnitTestElement>>("myTaskIdsToElements");
            var runElementsToTasks = run.GetField<Dictionary<IUnitTestElement, RemoteTask>>("myElementsToTasks");

            if (runTasks == null)
            {
                runTasks = new Dictionary<RemoteTask, IUnitTestElement>();
                run.SetField("myTasks", runTasks);
            }

            if (runTaskIdsToElements == null)
            {
                runTaskIdsToElements = new Dictionary<string, IUnitTestElement>();
                run.SetField("myTaskIdsToElements", runTaskIdsToElements);
            }

            if (runElementsToTasks == null)
            {
                runElementsToTasks = new Dictionary<IUnitTestElement, RemoteTask>();
                run.SetField("myElementsToTasks", runElementsToTasks);
            }

            foreach (var unitTestTask in sequence.GetAllTasksRecursive())
            {
                var element = originalRun.GetElementByRemoteTaskId(unitTestTask.Task.Id);
                
                runTasks[unitTestTask.Task] = element;

                if (element != null)
                {
                    runTaskIdsToElements[unitTestTask.Task.Id] = element;
                    runElementsToTasks[element] = unitTestTask.Task;
                }
            }

            run.GetRootTasks().Add(sequence);
            runTasks[sequence.Task] = silverlightElement;
            runTaskIdsToElements[sequence.Task.Id] = silverlightElement;
            runElementsToTasks[silverlightElement] = sequence.Task;
        }
    }
}