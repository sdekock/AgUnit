extern alias util;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.UnitTestExplorer.Launch;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper80.UnitTestFramework.SilverlightPlatform
{
    public static class SilverlightPlatformSupportExtensions
    {
        public static void EnsureSilverlightPlatformSupport(this IUnitTestLaunch launch, UnitTestProviders providers, ITaskRunnerHostController hostController)
        {
            var runs = launch.GetRuns();

            foreach (var run in runs.Values.Select(r => r.Value).ToArray())
            {
                foreach (var sequence in run.GetSequences().ToArray())
                {
                    ConvertToSilverlightSequenceIfNecessary(sequence, run, launch, providers, hostController);
                }
            }

            launch.RemoveEmptyRuns();
        }

        private static void ConvertToSilverlightSequenceIfNecessary(IList<UnitTestTask> sequence, UnitTestRun run, IUnitTestLaunch launch, UnitTestProviders providers, ITaskRunnerHostController hostController)
        {
            if (!sequence.IsSilverlightSequence())
            {
                var silverlightProject = sequence.GetSilverlightProject();
                if (silverlightProject != null)
                {
                    var silverlightRun = launch.GetOrCreateSilverlightRun(silverlightProject.PlatformID, providers, hostController);

                    sequence.AddSilverlightUnitTestTask(silverlightProject, providers, silverlightRun.Key.RunStrategy);

                    run.GetSequences().Remove(sequence);
                    silverlightRun.Value.AddTaskSequence(sequence);
                }
            }
        }
    }
}