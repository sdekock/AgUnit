using System;
using System.Collections.Generic;
using System.Linq;
using AgUnit.Runner.Resharper60.Util;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper60.UnitTestFramework.SilverlightPlatform
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
    }
}