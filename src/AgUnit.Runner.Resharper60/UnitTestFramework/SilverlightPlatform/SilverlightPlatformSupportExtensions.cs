using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestExplorer;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper60.UnitTestFramework.SilverlightPlatform
{
    public static class SilverlightPlatformSupportExtensions
    {
        public static void EnsureSilverlightPlatformSupport(this IUnitTestLaunch launch, UnitTestManager manager)
        {
            var runs = launch.GetRuns();

            foreach (var run in runs.Values.ToArray())
            {
                foreach (var sequence in run.GetSequences().ToArray())
                {
                    ConvertToSilverlightSequenceIfNecessary(sequence, run, launch, manager);
                }
            }

            launch.RemoveEmptyRuns();
        }

        private static void ConvertToSilverlightSequenceIfNecessary(IList<UnitTestTask> sequence, UnitTestRun run, IUnitTestLaunch launch, UnitTestManager manager)
        {
            if (!sequence.IsSilverlightSequence())
            {
                var requiredSilverlightPlatform = sequence.GetRequiredSilverlightPlatform();
                if (requiredSilverlightPlatform != null)
                {
                    run.GetSequences().Remove(sequence);

                    CreateSilverlightSequence(sequence, launch, manager, requiredSilverlightPlatform);
                }
            }
        }

        private static void CreateSilverlightSequence(IList<UnitTestTask> sequence, IUnitTestLaunch launch, UnitTestManager manager, PlatformID silverlightPlatform)
        {
            var silverlightRun = launch.GetOrCreateSilverlightRun(silverlightPlatform);

            sequence.AddSilverlightUnitTestTask(silverlightPlatform, manager);
            sequence.RemoveAssemblyLoadTasks();

            silverlightRun.AddTaskSequence(sequence);
        }
    }
}