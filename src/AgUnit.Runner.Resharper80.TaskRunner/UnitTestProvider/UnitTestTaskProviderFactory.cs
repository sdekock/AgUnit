using System.Collections.Generic;
using AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.MSTest;
using AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.nUnit;
using AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.XUnit;
using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider
{
    public static class UnitTestTaskProviderFactory
    {
        public static IEnumerable<IAssemblyTaskProvider> GetAssemblyTaskProviders()
        {
            return new IAssemblyTaskProvider[]
            {
                new MsTestAssemblyTaskProvider(),
                new NUnitAssemblyTaskProvider(),
                new XUnitAssemblyTaskProvider()
            };
        }

        public static IEnumerable<IClassTaskProvider> GetClassTaskProviders()
        {
            return new IClassTaskProvider[]
            {
                new MsTestClassTaskProvider(),
                new NUnitClassTaskProvider(),
                new XUnitClassTaskProvider()
            };
        }

        public static IEnumerable<IMethodTaskProvider> GetMethodTaskProviders()
        {
            return new IMethodTaskProvider[]
            {
                new MsTestMethodTaskProvider(),
                new NUnitMethodTaskProvider(),
                new XUnitMethodTaskProvider()
            };
        }
    }
}