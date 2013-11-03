using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestRunner.MSTest;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.MSTest
{
    public class MsTestClassTaskProvider : IClassTaskProvider
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