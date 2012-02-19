using System.Collections.Generic;
using AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight.Execution
{
    public class TaskEnvironment
    {
        public IRemoteTaskServer Server { get; private set; }
        public IEnumerable<IAssemblyTaskProvider> AssemblyTaskProviders { get; private set; }
        public IEnumerable<IClassTaskProvider> ClassTaskProviders { get; private set; }
        public IEnumerable<IMethodTaskProvider> MethodTaskProviders { get; private set; }

        public TaskEnvironment(IRemoteTaskServer server,
            IEnumerable<IAssemblyTaskProvider> assemblyTaskProviders,
            IEnumerable<IClassTaskProvider> classTaskProviders,
            IEnumerable<IMethodTaskProvider> methodTaskProviders)
        {
            Server = server;
            AssemblyTaskProviders = assemblyTaskProviders;
            ClassTaskProviders = classTaskProviders;
            MethodTaskProviders = methodTaskProviders;
        }
    }
}