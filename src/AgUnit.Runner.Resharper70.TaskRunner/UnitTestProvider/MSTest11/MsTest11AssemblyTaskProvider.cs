extern alias mstest11;
using AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;
using mstest11::JetBrains.ReSharper.UnitTestRunner.MSTest;

namespace AgUnit.Runner.Resharper70.TaskRunner.UnitTestProvider.MSTest11
{
    public class MsTest11AssemblyTaskProvider : IAssemblyTaskProvider
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