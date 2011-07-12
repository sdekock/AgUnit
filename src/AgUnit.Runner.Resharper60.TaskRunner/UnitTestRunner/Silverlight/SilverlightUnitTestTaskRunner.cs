using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestRunner.MSTest;
using StatLight.Client.Harness.Events;
using StatLight.Core.Common;
using StatLight.Core.Configuration;
using StatLight.Core.Events;
using StatLight.Core.Runners;
using StatLight.Core.WebBrowser;

namespace AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight
{
    public class SilverlightUnitTestTaskRunner : RecursiveRemoteTaskRunner
    {
        public SilverlightUnitTestTaskRunner(IRemoteTaskServer server)
            : base(server)
        { }

        public override TaskResult Start(TaskExecutionNode node)
        {
            return TaskResult.Success;
        }

        public override TaskResult Execute(TaskExecutionNode node)
        {
            return TaskResult.Success;
        }

        public override TaskResult Finish(TaskExecutionNode node)
        {
            return TaskResult.Success;
        }

        public override void ExecuteRecursive(TaskExecutionNode node)
        {
            Debugger.Break();

            try
            {
                Server.TaskStarting(node.RemoteTask);

                foreach (var assemblyTaskNode in node.Children)
                {
                    Execute(assemblyTaskNode, (MsTestTestAssemblyTask)assemblyTaskNode.RemoteTask);
                }

                Server.TaskFinished(node.RemoteTask, null, TaskResult.Success);
            }
            catch (Exception e)
            {
                Server.TaskException(node.RemoteTask, new[] { new TaskException(e) });
                Server.TaskFinished(node.RemoteTask, null, TaskResult.Exception);

                throw;
            }
        }

        private void Execute(TaskExecutionNode assemblyTaskNode, MsTestTestAssemblyTask assemblyTask)
        {
            try
            {
                Server.TaskStarting(assemblyTask);

                var xapPath = assemblyTask.AssemblyLocation.Replace(".dll", ".xap");
                var testMethods = assemblyTaskNode.GetTasks<MsTestTestMethodTask>().Select(m => m.GetFullMethodName()).ToList();

                ILogger logger = new ConsoleLogger(LogChatterLevels.Full);
                var eventAggregator = EventAggregatorFactory.Create(logger);

                // Create our custom listener and add it to the event aggregator 
                var reSharperTestingReportEventsHandler = new ReSharperTestingReportEventsHandler(Server, assemblyTaskNode, testMethods);
                eventAggregator.AddListener(reSharperTestingReportEventsHandler);

                var statLightConfigurationFactory = new StatLightConfigurationFactory(logger);

                var config = statLightConfigurationFactory.GetStatLightConfigurationForXap(
                    unitTestProviderType: UnitTestProviderType.Undefined, // Let StatLight figure it out
                    xapPath: xapPath,
                    microsoftTestingFrameworkVersion: null, // Let StatLight figure it out
                    methodsToTest: new Collection<string>(testMethods),
                    tagFilters: null,
                    numberOfBrowserHosts: 1, // Maybe you spin up 3 or 4 here if you know you're running a ton of tests
                    isRemoteRun: false,
                    queryString: "", // This is passed to the browser host page (say your test need some configuration - could be passed here - probably not a use case in ReSharper runner)
                    webBrowserType: WebBrowserType.SelfHosted,
                    forceBrowserStart: false,
                    showTestingBrowserHost: false // If you need UI support this needs to be true
                    );

                var statLightRunnerFactory = new StatLightRunnerFactory(logger, eventAggregator, eventAggregator);

                var onetimeConsoleRunner = statLightRunnerFactory.CreateOnetimeConsoleRunner(config);

                // This will be the blocking/slow operation that runs the tests...
                var testReport = onetimeConsoleRunner.Run();

                reSharperTestingReportEventsHandler.ReportIgnoredMethods();
                reSharperTestingReportEventsHandler.ReportOtherTasks();

                Server.TaskFinished(assemblyTask, null, TaskResult.Success);
            }
            catch (Exception e)
            {
                Server.TaskException(assemblyTask, new[] { new TaskException(e) });
                Server.TaskFinished(assemblyTask, null, TaskResult.Exception);

                throw;
            }
        }
    }

    public class ReSharperTestingReportEventsHandler : ITestingReportEvents
    {
        private readonly IRemoteTaskServer server;
        private readonly TaskExecutionNode rootNode;
        private readonly IList<string> methodsToRun;
        private readonly IList<RemoteTask> finishedTasks;

        public ReSharperTestingReportEventsHandler(IRemoteTaskServer server, TaskExecutionNode rootNode, IList<string> methodsToRun)
        {
            this.server = server;
            this.rootNode = rootNode;
            this.methodsToRun = methodsToRun;

            finishedTasks = new List<RemoteTask>();
        }

        public void Handle(TestCaseResult message)
        {
            if (message.MethodName != null)
            {
                var remoteTask = rootNode.GetTasks<MsTestTestMethodTask>().FirstOrDefault(t => t.GetFullMethodName() == message.FullMethodName());
                if (remoteTask != null)
                {
                    server.TaskStarting(remoteTask);
                    var output = message.ExceptionInfo != null ? message.ExceptionInfo.ToString() : null;
                    methodsToRun.Remove(message.FullMethodName());
                    server.TaskFinished(remoteTask, output, ToTaskResult(message.ResultType));
                    finishedTasks.Add(remoteTask);
                }
            }
        }

        private TaskResult ToTaskResult(ResultType resultType)
        {
            switch (resultType)
            {
                case ResultType.Passed:
                    return TaskResult.Success;
                case ResultType.Failed:
                    return TaskResult.Exception;
                case ResultType.Ignored:
                    return TaskResult.Skipped;
                case ResultType.SystemGeneratedFailure:
                    return TaskResult.Error;
            }

            throw new ArgumentOutOfRangeException();
        }

        public void Handle(TraceClientEvent message)
        {
            server.TaskOutput(rootNode.RemoteTask, message.Message, TaskOutputType.STDOUT);
        }

        public void Handle(BrowserHostCommunicationTimeoutServerEvent message)
        {
            server.TaskError(rootNode.RemoteTask, message.Message);
        }

        public void Handle(FatalSilverlightExceptionServerEvent message)
        {
            server.TaskError(rootNode.RemoteTask, message.Message);
        }

        public void Handle(UnhandledExceptionClientEvent message)
        {
            server.TaskError(rootNode.RemoteTask, message.ExceptionInfo.FullMessage);
        }

        public void ReportIgnoredMethods()
        {
            foreach (var method in methodsToRun)
            {
                var remoteTask = rootNode.GetTasks<MsTestTestMethodTask>().FirstOrDefault(t => t.GetFullMethodName() == method);
                if (remoteTask != null)
                {
                    server.TaskFinished(remoteTask, null, TaskResult.Skipped);
                    finishedTasks.Add(remoteTask);
                }
            }
        }

        public void ReportOtherTasks()
        {
            var allTaskNodes = rootNode.Children.FlattenNodesHierarchy().ToList();

            foreach (var taskNode in allTaskNodes.Where(t => !finishedTasks.Contains(t.RemoteTask)))
            {
                server.TaskFinished(taskNode.RemoteTask, null, TaskResult.Success);
            }
        }
    }

    public static class TaskExecutionNodeHelper
    {
        public static IEnumerable<T> GetTasks<T>(this TaskExecutionNode parentNode) where T : RemoteTask
        {
            return FlattenNodesHierarchy(new[] { parentNode }).Select(node => node.RemoteTask).OfType<T>();
        }

        public static IEnumerable<TaskExecutionNode> GetNodesWithTask<T>(this TaskExecutionNode parentNode) where T : RemoteTask
        {
            return FlattenNodesHierarchy(new[] { parentNode }).Where(node => node.RemoteTask is T);
        }

        public static IEnumerable<TaskExecutionNode> FlattenNodesHierarchy(this IEnumerable<TaskExecutionNode> nodes)
        {
            foreach (var node in nodes)
            {
                foreach (var childNode in FlattenNodesHierarchy(node.Children))
                {
                    yield return childNode;
                }

                yield return node;
            }
        }

        public static string GetFullMethodName(this MsTestTestMethodTask methodTask)
        {
            return string.Format("{0}.{1}", methodTask.TypeName, methodTask.ShortName);
        }
    }
}