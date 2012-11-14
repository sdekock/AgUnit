extern alias mstest11;
using AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;
using mstest11::JetBrains.ReSharper.UnitTestRunner.MSTest;

namespace AgUnit.Runner.Resharper70.TaskRunner.UnitTestProvider.MSTest11
{
    public class MsTest11ClassTaskProvider : IClassTaskProvider
    {
        public bool IsClassTask(RemoteTask task)
        {
            return task is MsTestTestClassTask;
        }

        private MsTestTestClassTask GetTask(RemoteTask task)
        {
            return (MsTestTestClassTask)task;
        }

        public string GetFullClassName(RemoteTask task)
        {
            var classTask = GetTask(task);

            return classTask.TypeName;
        }
    }
}