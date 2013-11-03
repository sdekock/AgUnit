using System.Collections.Generic;
using System.Linq;
using AgUnit.Runner.Resharper80.UnitTestFramework.Silverlight;
using AgUnit.Runner.Resharper80.Util;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestExplorer.Launch;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Strategy;

namespace AgUnit.Runner.Resharper80.UnitTestFramework.SilverlightPlatform
{
    public static class UnitTestLaunchExtensions
    {
        public static void RemoveEmptyRuns(this IUnitTestLaunch launch)
        {
            var runs = launch.GetRuns();
            var emptyRuns = runs.Values.Select(run => run.Value).Where(run => !run.GetSequences().Any()).ToArray();

            foreach (var run in emptyRuns)
            {
                runs.Remove(run.ID);
            }
        }

        public static KeyValuePair<UnitTestRunProperties, UnitTestRun> GetOrCreateSilverlightRun(this IUnitTestLaunch launch, PlatformID silverlightPlatform, UnitTestProviders providers, ITaskRunnerHostController hostController)
        {
            var runs = launch.GetRuns();
            var silverlightRun = runs.Values.FirstOrDefault(run => run.Value.GetSilverlightPlatformVersion() == silverlightPlatform.Version);

            if (silverlightRun.Value == null)
            {
                var runtimeEnvironment = new RuntimeEnvironment { PlatformType = PlatformType.x86, PlatformVersion = PlatformVersion.v4_0 };

                var run = new UnitTestRun((UnitTestLaunch)launch, runtimeEnvironment);
                var runStrategy = new OutOfProcessUnitTestRunStrategy(SilverlightUnitTestProvider.GetTaskRunnerInfo(launch));

                var unitTestProvider = providers.GetProvider(SilverlightUnitTestProvider.RunnerId);
                var runProperties = new UnitTestRunProperties(unitTestProvider, null, runStrategy, runtimeEnvironment);
                runProperties.RunController = hostController;

                silverlightRun = new KeyValuePair<UnitTestRunProperties, UnitTestRun>(runProperties, run);

                runs.Add(run.ID, silverlightRun);
            }

            return silverlightRun;
        }

        public static Dictionary<string, KeyValuePair<UnitTestRunProperties, UnitTestRun>> GetRuns(this IUnitTestLaunch launch)
        {
            return launch.GetField<Dictionary<string, KeyValuePair<UnitTestRunProperties, UnitTestRun>>>("myRuns");
        }
    }
}