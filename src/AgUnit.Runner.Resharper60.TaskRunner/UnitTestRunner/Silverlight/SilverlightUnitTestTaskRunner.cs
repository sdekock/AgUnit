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
using TinyIoC;

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

            var inputOptions = CreateInputOptions(silverlightTask, testMethods);

            // Bootstrap StatLight and load up needed dependencies.
            TinyIoCContainer ioc = StatLight.Core.BootStrapper.Initialize(inputOptions, logger);
            var eventSubscriptionManager = ioc.Resolve<IEventSubscriptionManager>();
            var statLightConfigurationFactory = ioc.Resolve<StatLightConfigurationFactory>();
            var statLightRunnerFactory = ioc.Resolve<StatLightRunnerFactory>();

            // Create the AgUnit specific test result handler and include it with the StatLight event aggregator.
            var eventsHandler = new SilverlightResultsHandler(testClasses, testMethods);
            eventSubscriptionManager.AddListener(eventsHandler);

            StatLightConfiguration statLightConfiguration = statLightConfigurationFactory.GetConfigurations().Single();
            IRunner onetimeConsoleRunner = statLightRunnerFactory.CreateOnetimeConsoleRunner(statLightConfiguration);
            var testReport = onetimeConsoleRunner.Run();
        }

        private static InputOptions CreateInputOptions(SilverlightTask silverlightTask, MethodTask[] testMethods)
        {
            var inputOptions = new InputOptions()
                .SetMethodsToTest(testMethods.Select(m => m.GetFullMethodName()))
                ;

            if (silverlightTask.HasXapPath())
                inputOptions.SetXapPaths(new[] {silverlightTask.GetXapPath()});
            else
                inputOptions.SetDllPaths(new[] {silverlightTask.GetDllPath()});
            return inputOptions;
        }

        private static DebugLogger CreateStatLightLogger()
        {
            return new DebugLogger(LogChatterLevels.Full);
        }
    }
}