using AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestRunner.nUnit;

namespace AgUnit.Runner.Resharper60.TaskRunner.UnitTestProvider.nUnit
{
    public class NUnitMethodTaskProvider : IMethodTaskProvider
    {
        public bool IsMethodTask(RemoteTask task)
        {
            return task is NUnitTestTask;
        }

        private NUnitTestTask GetTask(RemoteTask task)
        {
            return (NUnitTestTask)task;
        }

        public string GetFullMethodName(RemoteTask task)
        {
            var methodTask = GetTask(task);

            return string.Format("{0}.{1}", methodTask.TypeName, methodTask.ShortName);
        }
    }
}