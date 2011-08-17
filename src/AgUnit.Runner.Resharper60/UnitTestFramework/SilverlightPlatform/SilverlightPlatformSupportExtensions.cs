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
                var silverlightProject = sequence.GetSilverlightProject();
                if (silverlightProject != null)
                {
                    run.GetSequences().Remove(sequence);

                    CreateSilverlightSequence(sequence, launch, manager, silverlightProject);
                }
            }
        }

        private static void CreateSilverlightSequence(IList<UnitTestTask> sequence, IUnitTestLaunch launch, UnitTestManager manager, IProject silverlightProject)
        {
            var silverlightRun = launch.GetOrCreateSilverlightRun(silverlightProject.PlatformID);

            sequence.AddSilverlightUnitTestTask(silverlightProject, manager);
            sequence.RemoveAssemblyLoadTasks();

            silverlightRun.AddTaskSequence(sequence);
        }
    }
}