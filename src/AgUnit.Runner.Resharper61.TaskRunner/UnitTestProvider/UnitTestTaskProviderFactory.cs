using System.Collections.Generic;
using AgUnit.Runner.Resharper61.TaskRunner.UnitTestProvider.MSTest;
#if RS70
using AgUnit.Runner.Resharper70.TaskRunner.UnitTestProvider.MSTest11;
#endif
using AgUnit.Runner.Resharper61.TaskRunner.UnitTestProvider.XUnit;
using AgUnit.Runner.Resharper61.TaskRunner.UnitTestProvider.nUnit;
using AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight.Providers;

namespace AgUnit.Runner.Resharper61.TaskRunner.UnitTestProvider
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
#if RS70
                , new MsTest11AssemblyTaskProvider()
#endif
            };
        }

        public static IEnumerable<IClassTaskProvider> GetClassTaskProviders()
        {
            return new IClassTaskProvider[]
            {
                new MsTestClassTaskProvider(),
                new NUnitClassTaskProvider(),
                new XUnitClassTaskProvider()
#if RS70
                , new MsTest11ClassTaskProvider()
#endif
            };
        }

        public static IEnumerable<IMethodTaskProvider> GetMethodTaskProviders()
        {
            return new IMethodTaskProvider[]
            {
                new MsTestMethodTaskProvider(),
                new NUnitMethodTaskProvider(),
                new XUnitMethodTaskProvider()
#if RS70
                , new MsTest11MethodTaskProvider()
#endif
            };
        }
    }
}