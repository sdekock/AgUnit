extern alias mstest11;
using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.MSTest11
{
    public class MsTest11MethodTaskProvider : IMethodTaskProvider
    {
        public bool IsMethodTask(RemoteTask task)
        {
            return task is mstest11::JetBrains.ReSharper.UnitTestRunner.MSTest.MsTestTestMethodTask;
        }

        private mstest11::JetBrains.ReSharper.UnitTestRunner.MSTest.MsTestTestMethodTask GetTask(RemoteTask task)
        {
            return (mstest11::JetBrains.ReSharper.UnitTestRunner.MSTest.MsTestTestMethodTask)task;
        }

        public string GetFullMethodName(RemoteTask task)
        {
            var methodTask = GetTask(task);

            return string.Format("{0}.{1}", methodTask.TypeName, methodTask.ShortName);
        }
    }
}