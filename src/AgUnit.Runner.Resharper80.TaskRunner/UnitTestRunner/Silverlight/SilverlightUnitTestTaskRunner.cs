using System;
using System.Collections.Generic;
using System.Linq;
using AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider;
using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Execution;
using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.Util;
using StatLight.Core;
using StatLight.Core.Common.Logging;
using StatLight.Core.Configuration;
using StatLight.Core.Events;
using StatLight.Core.Reporting;
using TinyIoC;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight
{
    public class SilverlightUnitTestTaskRunner : RecursiveRemoteTaskRunner
    {
        public SilverlightUnitTestTaskRunner(IRemoteTaskServer server)
            : base(server)
        { }
        
        public override void ExecuteRecursive(TaskExecutionNode node)
        {
            //System.Diagnostics.Debugger.Launch();

            try
            {
                ExecuteSilverlightTasks(node);
            }
            catch (Exception e)
            {
                MessageBox.ShowError(e.ToString(), "AgUnit: Exception during test run");
                throw;
            }
        }

        private void ExecuteSilverlightTasks(TaskExecutionNode node)
        {
            var assemblyTaskProviders = UnitTestTaskProviderFactory.GetAssemblyTaskProviders();
            var classTaskProviders = UnitTestTaskProviderFactory.GetClassTaskProviders();
            var methodTaskProviders = UnitTestTaskProviderFactory.GetMethodTaskProviders();

            var taskEnvironment = new TaskEnvironment(Server, assemblyTaskProviders, classTaskProviders, methodTaskProviders);
            var taskNode = new TaskNode(node, taskEnvironment);

            foreach (var silverlightTaskNode in taskNode.GetSilverlightTasks())
            {
                silverlightTaskNode.Execute(Execute);
            }
        }

        private void Execute(SilverlightTask silverlightTask)
        {
            var testMethods = silverlightTask.Node.GetMethodTasks().ToArray();
            var testClasses = silverlightTask.Node.GetClassTasks().ToArray();

            var ioc = BootStrapStatLight(silverlightTask, testMethods);
            SetUpSilverlightResultsHandler(ioc, testClasses, testMethods);

            var testReports = ExecuteStatLightRun(ioc);
        }

        private static TestReportCollection ExecuteStatLightRun(TinyIoCContainer ioc)
        {
            var commandLineExecutionEngine = ioc.Resolve<RunnerExecutionEngine>();

            return commandLineExecutionEngine.Run();
        }

        private static TinyIoCContainer BootStrapStatLight(SilverlightTask silverlightTask, MethodTask[] testMethods)
        {
            var inputOptions = CreateStatLightInputOptions(silverlightTask, testMethods);
            var logger = CreateStatLightLogger();

            return BootStrapper.Initialize(inputOptions, logger);
        }

        private static DebugLogger CreateStatLightLogger()
        {
            return new DebugLogger(LogChatterLevels.Full);
        }

        private static InputOptions CreateStatLightInputOptions(SilverlightTask silverlightTask, MethodTask[] testMethods)
        {
            return new InputOptions()
                .SetXapPaths(silverlightTask.GetXapPaths())
                .SetDllPaths(silverlightTask.GetDllPaths())
                .SetMethodsToTest(testMethods.Select(m => m.GetFullMethodName()).ToList());
        }

        private static void SetUpSilverlightResultsHandler(TinyIoCContainer ioc, IEnumerable<ClassTask> testClasses, IEnumerable<MethodTask> testMethods)
        {
            var silverlightResultsHandler = new SilverlightResultsHandler(testClasses, testMethods);
            var eventSubscriptionManager = ioc.Resolve<IEventSubscriptionManager>();

            eventSubscriptionManager.AddListener(silverlightResultsHandler);
        }
    }
}