extern alias util;
using System;
using System.Runtime.InteropServices;
using AgUnit.Runner.Resharper60.UnitTestFramework.SilverlightPlatform;
using AgUnit.Runner.Resharper60.Util;
using EnvDTE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestExplorer;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Threading;
using Microsoft.VisualStudio.Shell.Interop;
using util::JetBrains.Util;
using Thread = System.Threading.Thread;

namespace AgUnit.Runner.Resharper60.UnitTestFramework
{
    public class ExtendedDebugTaskRunnerHostController : DebugTaskRunnerHostController
    {
        public ExtendedDebugTaskRunnerHostController(IUnitTestSessionManager sessionManager, util::JetBrains.Util.Lazy.Lazy<IVsDebugger2> debugger2, DTE dte, IThreading threading, IUnitTestLaunch launch, string remotingAddress)
            : base(sessionManager, debugger2, dte, threading, launch, remotingAddress)
        { }

        public override void Run(string remotingAddress, IUnitTestRun run)
        {
            if (run.IsSilverlightRun())
            {
                RunWithSilverlightDebugger(remotingAddress, run);
            }
            else
            {
                base.Run(remotingAddress, run);
            }
        }

        private void RunWithSilverlightDebugger(string remotingAddress, IUnitTestRun run)
        {
            var targetInfo = CreateTargetInfo(remotingAddress, run);

            this.SetField("myRunId", run.ID);
            this.SetField("myTargetInfo", targetInfo);

            new Thread(CallThreadProc) { IsBackground = true }.Start();
        }

        private VsDebugTargetInfo2 CreateTargetInfo(string remotingAddress, IUnitTestRun run)
        {
            var runnerPath = GetTaskRunnerPathForRun(run);
            var runnerArgs = GetTaskRunnerCommandLineArgs(remotingAddress, run.ID);
            var silverlightDebugEngineGuid = new Guid("032F4B8C-7045-4B24-ACCF-D08C9DA108FE");

            var debugTargetInfo = new VsDebugTargetInfo2
            {
                dlo = 1U,
                bstrExe = runnerPath.FullPath,
                bstrCurDir = runnerPath.Directory.FullPath,
                bstrArg = runnerArgs,
                guidLaunchDebugEngine = silverlightDebugEngineGuid,
                LaunchFlags = 97U
            };

            debugTargetInfo.cbSize = (uint)Marshal.SizeOf(debugTargetInfo);

            return debugTargetInfo;
        }

        private void CallThreadProc()
        {
            Logger.Catch(() => this.CallMethod("ThreadProc"));
        }
    }
}