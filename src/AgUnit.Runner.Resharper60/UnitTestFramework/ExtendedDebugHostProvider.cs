extern alias util;
using AgUnit.Runner.Resharper60.Util;
using EnvDTE;
using JetBrains.ReSharper.UnitTestExplorer;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Threading;
using Microsoft.VisualStudio.Shell.Interop;
using util::JetBrains.Util.Lazy;

namespace AgUnit.Runner.Resharper60.UnitTestFramework
{
    public class ExtendedDebugHostProvider : HostProviderWrapper<DebugHostProvider>
    {
        private readonly Lazy<IVsDebugger2> debugger2;
        private readonly DTE dte;
        private readonly IThreading threading;

        public ExtendedDebugHostProvider(DebugHostProvider wrappedHostProvider)
            : base(wrappedHostProvider)
        {
            debugger2 = wrappedHostProvider.GetField<Lazy<IVsDebugger2>>("myDebugger");
            dte = wrappedHostProvider.GetField<DTE>("myDte");
            threading = wrappedHostProvider.GetField<IThreading>("myThreading");
        }

        protected override ITaskRunnerHostController CreateWrappedHostController(UnitTestManager manager, UnitTestSessionManager sessionManager, IUnitTestLaunch launch, string remotingAddress)
        {
            return new ExtendedDebugTaskRunnerHostController(manager, sessionManager, debugger2, dte, threading, launch, remotingAddress);
        }
    }
}