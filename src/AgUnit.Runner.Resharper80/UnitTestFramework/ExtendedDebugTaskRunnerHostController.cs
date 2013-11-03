extern alias util;
using System;
using System.Runtime.InteropServices;
using AgUnit.Runner.Resharper80.UnitTestFramework.SilverlightPlatform;
using AgUnit.Runner.Resharper80.Util;
using EnvDTE;
using JetBrains.ReSharper.UnitTestExplorer.Manager;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Threading;
using Microsoft.VisualStudio.Shell.Interop;
using Thread = System.Threading.Thread;

namespace AgUnit.Runner.Resharper80.UnitTestFramework
{
    public class ExtendedDebugTaskRunnerHostController : DebugTaskRunnerHostController
    {
        private readonly int port;

        public ExtendedDebugTaskRunnerHostController(IUnitTestLaunchManager launchManager, IUnitTestAgentManager agentManager, util::JetBrains.Util.Lazy.Lazy<IVsDebugger2> debugger2, DTE dte, IThreading threading, IUnitTestLaunch launch, int port)
            : base(launchManager, agentManager, debugger2, dte, threading, launch, port)
        {
            this.port = port;
        }

        public override void Run(IUnitTestRun run)
        {
            if (run.IsSilverlightRun())
            {
                RunWithSilverlightDebugger(run);
            }
            else
            {
                base.Run(run);
            }
        }

        private void RunWithSilverlightDebugger(IUnitTestRun run)
        {
            var targetInfo = CreateTargetInfo(run);

            this.SetField("myRunId", run.ID);
            this.SetField("myTargetInfo", targetInfo);

            new Thread(CallThreadProc) { IsBackground = true }.Start();
        }

        private VsDebugTargetInfo2 CreateTargetInfo(IUnitTestRun run)
        {
            var runnerPath = GetTaskRunnerPathForRun(run);
            var runnerArgs = GetTaskRunnerCommandLineArgs(run.ID, port);
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
            util::JetBrains.Threading.ThreadManager.Instance.ExecuteTask(() => util::JetBrains.Util.Logging.Logger.Catch(() => this.CallMethod("ThreadProc")));
        }
    }
}