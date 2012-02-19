extern alias util;
using AgUnit.Runner.Resharper61.Util;
using EnvDTE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestExplorer;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Threading;
using Microsoft.VisualStudio.Shell.Interop;

namespace AgUnit.Runner.Resharper61.UnitTestFramework
{
    public class ExtendedDebugHostProvider : HostProviderWrapper<DebugHostProvider>
    {
        private readonly util::JetBrains.Util.Lazy.Lazy<IVsDebugger2> debugger2;
        private readonly DTE dte;
        private readonly IThreading threading;

        public ExtendedDebugHostProvider(DebugHostProvider wrappedHostProvider)
            : base(wrappedHostProvider)
        {
            debugger2 = wrappedHostProvider.GetField<util::JetBrains.Util.Lazy.Lazy<IVsDebugger2>>("myDebugger");
            dte = wrappedHostProvider.GetField<DTE>("myDte");
            threading = wrappedHostProvider.GetField<IThreading>("myThreading");
        }

        protected override ITaskRunnerHostController CreateWrappedHostController(ISolution solution, IUnitTestSessionManager sessionManager, IUnitTestLaunch launch, string remotingAddress)
        {
            return new ExtendedDebugTaskRunnerHostController(sessionManager, debugger2, dte, threading, launch, remotingAddress);
        }
    }
}