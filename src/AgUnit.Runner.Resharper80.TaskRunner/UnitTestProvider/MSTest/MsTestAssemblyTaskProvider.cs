using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestRunner.MSTest;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.MSTest
{
    public class MsTestAssemblyTaskProvider : IAssemblyTaskProvider
    {
        public bool IsAssemblyTask(RemoteTask task)
        {
            return task is MsTestTestAssemblyTask;
        }

        private MsTestTestAssemblyTask GetTask(RemoteTask task)
        {
            return (MsTestTestAssemblyTask)task;
        }

        public string GetAssemblyLocation(RemoteTask task)
        {
            var assemblyTask = GetTask(task);

            return assemblyTask.AssemblyLocation;
        }
    }
}