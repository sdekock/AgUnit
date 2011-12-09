using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AgUnit.Runner.Resharper60.TaskRunner.UnitTestProvider.MSTest;
using AgUnit.Runner.Resharper60.TaskRunner.UnitTestProvider.XUnit;
using AgUnit.Runner.Resharper60.TaskRunner.UnitTestProvider.nUnit;
using AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Execution;
using AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;
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
            //Debugger.Break();

            var assemblyProviders = new IAssemblyTaskProvider[]
            {
                new MsTestAssemblyTaskProvider(),
                new NUnitAssemblyTaskProvider(),
                new XUnitAssemblyTaskProvider()
            };
            var classProviders = new IClassTaskProvider[]
            {
                new MsTestClassTaskProvider(),
                new NUnitClassTaskProvider(),
                new XUnitClassTaskProvider()
            };
            var methodProviders = new IMethodTaskProvider[]
            {
                new MsTestMethodTaskProvider(),
                new NUnitMethodTaskProvider(),
                new XUnitMethodTaskProvider()
            };

            var taskEnvironment = new TaskEnvironment(Server, assemblyProviders, classProviders, methodProviders);
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

            var logger = CreateStatLightLogger();
            var eventAggregator = CreateStatLightEventAggregator(testClasses, testMethods, logger);
            var configuration = CreateStatLightConfiguration(silverlightTask, logger, testMethods);
            var runner = CreateStatLightRunner(configuration, logger, eventAggregator);

            var testReport = runner.Run();
        }

        private static DebugLogger CreateStatLightLogger()
        {
            return new DebugLogger(LogChatterLevels.Full);
        }

        private static EventAggregator CreateStatLightEventAggregator(IEnumerable<ClassTask> testClasses, IEnumerable<MethodTask> testMethods, ILogger logger)
        {
            var eventsHandler = new SilverlightResultsHandler(testClasses, testMethods);
            var eventAggregator = new EventAggregatorFactory(logger).Create();

            eventAggregator.AddListener(eventsHandler);

            return eventAggregator;
        }

        private static StatLightConfiguration CreateStatLightConfiguration(SilverlightTask silverlightTask, DebugLogger logger, MethodTask[] testMethods)
        {
            var inputOptions = new InputOptions()
                .SetMethodsToTest(testMethods.Select(m => m.GetFullMethodName()))
                ;

            if (silverlightTask.HasXapPath())
                inputOptions.SetXapPaths(new[] { silverlightTask.GetXapPath() });
            else
                inputOptions.SetDllPaths(new[] { silverlightTask.GetDllPath() });

            var configurationFactory = new StatLightConfigurationFactory(logger, inputOptions);

            return configurationFactory.GetConfigurations().Single();
        }

        private static IRunner CreateStatLightRunner(StatLightConfiguration config, ILogger logger, EventAggregator eventAggregator)
        {
            var runnerFactory = new StatLightRunnerFactory(logger, eventAggregator, eventAggregator);

            return runnerFactory.CreateOnetimeConsoleRunner(config);
        }
    }
}