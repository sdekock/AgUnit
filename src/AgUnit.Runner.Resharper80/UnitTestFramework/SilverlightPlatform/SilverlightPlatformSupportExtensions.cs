extern alias util;
using System.Linq;
using AgUnit.Runner.Resharper80.UnitTestFramework.Silverlight;
using JetBrains.ReSharper.TaskRunnerFramework;
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
                foreach (var sequence in run.GetRootTasks().ToArray())
                {
                    ConvertToSilverlightSequenceIfNecessary(sequence, run, launch, providers, hostController);
                }
            }

            launch.RemoveEmptyRuns();
        }

        private static void ConvertToSilverlightSequenceIfNecessary(RemoteTaskPacket sequence, IUnitTestRun run, IUnitTestLaunch launch, UnitTestProviders providers, ITaskRunnerHostController hostController)
        {
            if (!sequence.IsSilverlightSequence())
            {
                var silverlightProject = sequence.GetSilverlightProject(run);
                if (silverlightProject != null)
                {
                    var silverlightRun = launch.GetOrCreateSilverlightRun(silverlightProject.PlatformID, providers, hostController);

                    var provider = providers.GetProvider(SilverlightUnitTestProvider.RunnerId);
                    var silverlightElement = new SilverlightUnitTestElement(provider, silverlightRun.Key.RunStrategy);

                    var remoteTask = new SilverlightUnitTestTask(silverlightProject.PlatformID.Version, silverlightProject.GetXapPath(), silverlightProject.GetDllPath());

                    var silverlightSequence = new RemoteTaskPacket(remoteTask) { TaskPackets = { sequence } };

                    run.GetRootTasks().Remove(sequence);
                    silverlightRun.Value.AddTaskSequence(silverlightSequence, silverlightElement, run);
                }
            }
        }
    }
}