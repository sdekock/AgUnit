using System;
using System.Collections.Generic;
using System.Linq;
using AgUnit.Runner.Resharper60.Util;
using JetBrains.Application;
using JetBrains.ReSharper.UnitTestExplorer;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper60.UnitTestFramework
{
    [ShellComponent]
    public class HostProviderInterceptor
    {
        public HostProviderInterceptor()
        {
            var hostProviders = GetHostProviders();

            Wrap<ProcessHostProvider>(hostProviders, p => new HostProviderWrapper<ProcessHostProvider>(p));
            Wrap<DebugHostProvider>(hostProviders, p => new ExtendedDebugHostProvider(p));
        }

        private static List<IHostProvider> GetHostProviders()
        {
            var unitTestHost = UnitTestHost.Instance;
            var hostProviders = unitTestHost.GetField<IEnumerable<IHostProvider>>("myHostProviders").ToList();

            unitTestHost.SetField("myHostProviders", hostProviders);

            return hostProviders;
        }

        private static void Wrap<THostProvider>(IList<IHostProvider> hostProviders, Func<THostProvider, HostProviderWrapper<THostProvider>> createWrapper)
            where THostProvider : IHostProvider
        {
            for (var i = 0; i < hostProviders.Count; i++)
            {
                if (hostProviders[i] is THostProvider)
                {
                    hostProviders[i] = createWrapper((THostProvider)hostProviders[i]);
                }
            }
        }
    }
}