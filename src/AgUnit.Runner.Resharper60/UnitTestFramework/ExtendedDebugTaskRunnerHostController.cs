extern alias util;

using EnvDTE;
using JetBrains.ReSharper.UnitTestExplorer;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Threading;
using Microsoft.VisualStudio.Shell.Interop;
using util::JetBrains.Util.Lazy;

namespace AgUnit.Runner.Resharper60.UnitTestFramework
{
    public class ExtendedDebugTaskRunnerHostController : DebugTaskRunnerHostController
    {
        public ExtendedDebugTaskRunnerHostController(UnitTestManager manager, UnitTestSessionManager sessionManager, Lazy<IVsDebugger2> debugger2, DTE dte, IThreading threading, IUnitTestLaunch launch, string remotingAddress)
            : base(manager, sessionManager, debugger2, dte, threading, launch, remotingAddress)
        { }

        // TODO: If we have a Silverlight test run, we should attach the Silverlight debugger here instead of the .NET/COM debugger.
        // See the implementation in the version of AgUnit for R# 5.0: https://hg01.codeplex.com/agunit/file/e1d353fe2274/AgUnit.Runner.Resharper50/UnitTestHost/AgUnitHostController.cs
        public override void Run(string remotingAddress, IUnitTestRun run)
        {
            base.Run(remotingAddress, run);
        }
    }
}