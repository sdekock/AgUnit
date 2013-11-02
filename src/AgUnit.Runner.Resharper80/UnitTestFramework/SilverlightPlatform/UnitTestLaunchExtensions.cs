using System.Collections.Generic;
using System.Linq;
using AgUnit.Runner.Resharper61.UnitTestFramework.Silverlight;
using AgUnit.Runner.Resharper61.Util;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestExplorer;
#if RS80
using JetBrains.ReSharper.UnitTestFramework.Strategy;
using JetBrains.ReSharper.UnitTestExplorer.Launch;
#endif
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper61.UnitTestFramework.SilverlightPlatform
{
    public static class UnitTestLaunchExtensions
    {
#if RS80
        public static void RemoveEmptyRuns(this IUnitTestLaunch launch)
        {
            var runs = launch.GetRuns();
            var emptyRuns = runs.Values.Select(run => run.Value).Where(run => !run.GetSequences().Any()).ToArray();

            foreach (var run in emptyRuns)
            {
                runs.Remove(run.ID);
            }
        }

        public static UnitTestRun GetOrCreateSilverlightRun(this IUnitTestLaunch launch, PlatformID silverlightPlatform, UnitTestProviders providers)
        {
            var runs = launch.GetRuns();
            var silverlightRun = runs.Values.Select(run => run.Value).FirstOrDefault(run => run.GetSilverlightPlatformVersion() == silverlightPlatform.Version);

            if (silverlightRun == null)
            {
                var runtimeEnvironment = new RuntimeEnvironment { PlatformType = PlatformType.x86, PlatformVersion = PlatformVersion.v4_0 };

                silverlightRun = new UnitTestRun((UnitTestLaunch)launch, runtimeEnvironment);

                var runStrategy = new OutOfProcessUnitTestRunStrategy(SilverlightUnitTestProvider.GetTaskRunnerInfo());
                var unitTestProvider = providers.GetProvider(SilverlightUnitTestProvider.RunnerId);
                var runProperties = new UnitTestRunProperties(unitTestProvider, null, runStrategy, runtimeEnvironment);

                runs.Add(silverlightRun.ID, new KeyValuePair<UnitTestRunProperties, UnitTestRun>(runProperties, silverlightRun));
            }

            return silverlightRun;
        }

        public static Dictionary<string, KeyValuePair<UnitTestRunProperties, UnitTestRun>> GetRuns(this IUnitTestLaunch launch)
        {
            return launch.GetField<Dictionary<string, KeyValuePair<UnitTestRunProperties, UnitTestRun>>>("myRuns");
        }
#else
        public static void RemoveEmptyRuns(this IUnitTestLaunch launch)
        {
            var runs = launch.GetRuns();
            var emptyRuns = runs.Values.Where(run => !run.GetSequences().Any()).ToArray();

            foreach (var run in emptyRuns)
            {
                runs.Remove(run.ID);
            }
        }

        public static UnitTestRun GetOrCreateSilverlightRun(this IUnitTestLaunch launch, PlatformID silverlightPlatform, UnitTestProviders providers)
        {
            var runs = launch.GetRuns();
            var silverlightRun = runs.Values.FirstOrDefault(run => run.GetSilverlightPlatformVersion() == silverlightPlatform.Version);

            if (silverlightRun == null)
            {
                var runtimeEnvironment = new RuntimeEnvironment { PlatformType = PlatformType.x86, PlatformVersion = PlatformVersion.v4_0 };
                silverlightRun = new UnitTestRun((UnitTestLaunch)launch, runtimeEnvironment);
                runs.Add(silverlightRun.ID, silverlightRun);
            }

            return silverlightRun;
        }

        public static Dictionary<string, UnitTestRun> GetRuns(this IUnitTestLaunch launch)
        {
            return launch.GetField<Dictionary<string, UnitTestRun>>("myRuns");
        }
#endif
    }
}