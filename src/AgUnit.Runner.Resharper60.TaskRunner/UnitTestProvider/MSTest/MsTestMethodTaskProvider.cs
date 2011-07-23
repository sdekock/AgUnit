using AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestRunner.MSTest;

namespace AgUnit.Runner.Resharper60.TaskRunner.UnitTestProvider.MSTest
{
    public class MsTestMethodTaskProvider : IMethodTaskProvider
    {
        public bool IsMethodTask(RemoteTask task)
        {
            return task is MsTestTestMethodTask;
        }

        private MsTestTestMethodTask GetTask(RemoteTask task)
        {
            return (MsTestTestMethodTask)task;
        }

        public string GetFullMethodName(RemoteTask task)
        {
            var methodTask = GetTask(task);

            return string.Format("{0}.{1}", methodTask.TypeName, methodTask.ShortName);
        }
    }
}