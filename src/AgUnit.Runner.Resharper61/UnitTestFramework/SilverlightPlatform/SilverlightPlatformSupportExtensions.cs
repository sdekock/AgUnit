using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestExplorer;
#if RS80
using JetBrains.ReSharper.UnitTestExplorer.Launch;
#endif
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper61.UnitTestFramework.SilverlightPlatform
{
    public static class SilverlightPlatformSupportExtensions
    {
        public static void EnsureSilverlightPlatformSupport(this IUnitTestLaunch launch, UnitTestProviders providers)
        {
            var runs = launch.GetRuns();

#if RS80
            foreach (var run in runs.Values.Select(r => r.Value).ToArray())
#else
            foreach (var run in runs.Values.ToArray())
#endif
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
            var silverlightRun = launch.GetOrCreateSilverlightRun(silverlightProject.PlatformID, providers);

            sequence.AddSilverlightUnitTestTask(silverlightProject, providers);
            sequence.RemoveAssemblyLoadTasks();

            silverlightRun.AddTaskSequence(sequence);
        }
    }
}