using System;
using System.Collections.Generic;
using System.Linq;
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
            var sequence = run.GetSequences().FirstOrDefault();
            return sequence != null ? sequence.GetSilverlightPlatformVersion() : null;
        }

        public static IList<IList<UnitTestTask>> GetSequences(this IUnitTestRun run)
        {
            return run.GetField<IList<IList<UnitTestTask>>>("mySequences");
        }

        public static void AddTaskSequence(this IUnitTestRun run, IList<UnitTestTask> sequence)
        {
            var runTasks = run.GetField<Dictionary<RemoteTask, IUnitTestElement>>("myTasks");
            var runTaskIdsToElements = run.GetField<Dictionary<string, IUnitTestElement>>("myTaskIdsToElements");

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

            foreach (var unitTestTask in sequence)
            {
                runTasks[unitTestTask.RemoteTask] = unitTestTask.Element;

                if (unitTestTask.Element != null)
                    runTaskIdsToElements[unitTestTask.RemoteTask.Id] = unitTestTask.Element;
            }

            run.GetSequences().Add(sequence);
        }
    }
}