using System.Collections.Generic;
using System.Linq;
using AgUnit.Runner.Resharper60.Util;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestExplorer;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper60.UnitTestFramework.SilverlightPlatform
{
    public static class UnitTestLaunchExtensions
    {
        public static void RemoveEmptyRuns(this IUnitTestLaunch launch)
        {
            var runs = launch.GetRuns();
            var emptyRuns = runs.Values.Where(run => !run.GetSequences().Any()).ToArray();

            foreach (var run in emptyRuns)
            {
                runs.Remove(run.ID);
            }
        }

        public static UnitTestRun GetOrCreateSilverlightRun(this IUnitTestLaunch launch, PlatformID silverlightPlatform)
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
    }
}