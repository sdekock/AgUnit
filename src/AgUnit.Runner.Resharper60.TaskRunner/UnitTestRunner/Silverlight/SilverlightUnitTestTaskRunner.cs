using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using AgUnit.Runner.Resharper60.TaskRunner.UnitTestProvider.MSTest;
using AgUnit.Runner.Resharper60.TaskRunner.UnitTestProvider.XUnit;
using AgUnit.Runner.Resharper60.TaskRunner.UnitTestProvider.nUnit;
using AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Execution;
using AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Providers;
using EventAggregatorNet;
using JetBrains.ReSharper.TaskRunnerFramework;
using StatLight.Core.Common;
using StatLight.Core.Configuration;
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

            foreach (var assemblyTaskNode in taskNode.GetAssemblyTasks())
            {
                assemblyTaskNode.Execute(Execute);
            }
        }

        private void Execute(AssemblyTask assemblyTask)
        {
            var xapPath = assemblyTask.GetXapPath();
            var testMethods = assemblyTask.Node.GetMethodTasks().ToArray();
            var testClasses = assemblyTask.Node.GetClassTasks().ToArray();

            var logger = CreateStatLightLogger();
            var eventAggregator = CreateStatLightEventAggregator(testClasses, testMethods, logger);
            var configuration = CreateStatLightConfiguration(testMethods, logger, xapPath);
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
            var eventAggregator = EventAggregatorFactory.Create(logger);

            eventAggregator.AddListener(eventsHandler);

            return eventAggregator;
        }

        private static StatLightConfiguration CreateStatLightConfiguration(IEnumerable<MethodTask> testMethods, ILogger logger, string xapPath)
        {
            var configurationFactory = new StatLightConfigurationFactory(logger);

            return configurationFactory.GetStatLightConfigurationForXap(
                unitTestProviderType: UnitTestProviderType.Undefined, // Let StatLight figure it out
                xapPath: xapPath,
                microsoftTestingFrameworkVersion: null, // Let StatLight figure it out
                methodsToTest: new Collection<string>(testMethods.Select(m => m.GetFullMethodName()).ToList()),
                tagFilters: null,
                numberOfBrowserHosts: 1, // Maybe you spin up 3 or 4 here if you know you're running a ton of tests
                isRemoteRun: false,
                queryString: "", // This is passed to the browser host page (say your test need some configuration - could be passed here - probably not a use case in ReSharper runner)
                webBrowserType: WebBrowserType.SelfHosted,
                forceBrowserStart: false,
                showTestingBrowserHost: false // If you need UI support this needs to be true
            );
        }

        private static IRunner CreateStatLightRunner(StatLightConfiguration config, ILogger logger, EventAggregator eventAggregator)
        {
            var runnerFactory = new StatLightRunnerFactory(logger, eventAggregator, eventAggregator);

            return runnerFactory.CreateOnetimeConsoleRunner(config);
        }
    }
}