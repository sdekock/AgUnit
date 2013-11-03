extern alias util;
using AgUnit.Runner.Resharper80.Util;
using EnvDTE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestExplorer.Manager;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Threading;
using Microsoft.VisualStudio.Shell.Interop;

namespace AgUnit.Runner.Resharper80.UnitTestFramework
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

        protected override ITaskRunnerHostController CreateWrappedHostController(ISolution solution, IUnitTestLaunchManager launchManager, IUnitTestAgentManager agentManager, IUnitTestLaunch launch)
        {
            return new ExtendedDebugTaskRunnerHostController(launchManager, agentManager, debugger2, dte, threading, launch, solution.GetComponent<UnitTestServer>().PortNumber);
        }
    }
}