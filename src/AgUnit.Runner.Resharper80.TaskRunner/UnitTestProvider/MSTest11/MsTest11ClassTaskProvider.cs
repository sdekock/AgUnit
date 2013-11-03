extern alias mstest11;
using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.MSTest11
{
    public class MsTest11ClassTaskProvider : IClassTaskProvider
    {
        public bool IsClassTask(RemoteTask task)
        {
            return task is mstest11::JetBrains.ReSharper.UnitTestRunner.MSTest.MsTestTestClassTask;
        }

        private mstest11::JetBrains.ReSharper.UnitTestRunner.MSTest.MsTestTestClassTask GetTask(RemoteTask task)
        {
            return (mstest11::JetBrains.ReSharper.UnitTestRunner.MSTest.MsTestTestClassTask)task;
        }

        public string GetFullClassName(RemoteTask task)
        {
            var classTask = GetTask(task);

            return classTask.TypeName;
        }
    }
}