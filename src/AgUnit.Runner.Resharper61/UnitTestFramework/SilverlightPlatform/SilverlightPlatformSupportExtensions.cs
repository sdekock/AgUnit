using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestExplorer;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper60.UnitTestFramework.SilverlightPlatform
{
    public static class SilverlightPlatformSupportExtensions
    {
        public static void EnsureSilverlightPlatformSupport(this IUnitTestLaunch launch, UnitTestProviders providers)
        {
            var runs = launch.GetRuns();

            foreach (var run in runs.Values.ToArray())
            {
                foreach (var sequence in run.GetSequences().ToArray())
                {
                    ConvertToSilverlightSequenceIfNecessary(sequence, run, launch, providers);
                }
            }

            launch.RemoveEmptyRuns();
        }

        private static void ConvertToSilverlightSequenceIfNecessary(IList<UnitTestTask> sequence, UnitTestRun run, IUnitTestLaunch launch, UnitTestProviders providers)
        {
            if (!sequence.IsSilverlightSequence())
            {
                var silverlightProject = sequence.GetSilverlightProject();
                if (silverlightProject != null)
                {
                    run.GetSequences().Remove(sequence);

                    CreateSilverlightSequence(sequence, launch, providers, silverlightProject);
                }
            }
        }

        private static void CreateSilverlightSequence(IList<UnitTestTask> sequence, IUnitTestLaunch launch, UnitTestProviders providers, IProject silverlightProject)
        {
            var silverlightRun = launch.GetOrCreateSilverlightRun(silverlightProject.PlatformID);

            sequence.AddSilverlightUnitTestTask(silverlightProject, providers);
            sequence.RemoveAssemblyLoadTasks();

            silverlightRun.AddTaskSequence(sequence);
        }
    }
}