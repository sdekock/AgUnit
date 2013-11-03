extern alias mstest11;
using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.MSTest11
{
    public class MsTest11AssemblyTaskProvider : IAssemblyTaskProvider
    {
        public bool IsAssemblyTask(RemoteTask task)
        {
            return task is mstest11::JetBrains.ReSharper.UnitTestRunner.MSTest.MsTestTestAssemblyTask;
        }

        private mstest11::JetBrains.ReSharper.UnitTestRunner.MSTest.MsTestTestAssemblyTask GetTask(RemoteTask task)
        {
            return (mstest11::JetBrains.ReSharper.UnitTestRunner.MSTest.MsTestTestAssemblyTask)task;
        }

        public string GetAssemblyLocation(RemoteTask task)
        {
            var assemblyTask = GetTask(task);

            return assemblyTask.AssemblyLocation;
        }
    }
}