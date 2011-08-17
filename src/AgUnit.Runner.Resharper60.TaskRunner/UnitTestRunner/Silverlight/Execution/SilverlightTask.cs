using System;
using AgUnit.Runner.Resharper60.UnitTestFramework.Silverlight;

namespace AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Execution
{
    public class SilverlightTask
    {
        public TaskNode Node { get; private set; }

        public SilverlightTask(TaskNode node)
        {
            Node = node;
        }

        public void Execute(Action<SilverlightTask> execute)
        {
            Node.Execute(execute, this);
        }

        private SilverlightUnitTestTask GetTask()
        {
            return (SilverlightUnitTestTask)Node.Task;
        }

        public bool HasXapPath()
        {
            return !string.IsNullOrWhiteSpace(GetXapPath());
        }

        public string GetXapPath()
        {
            return GetTask().XapPath;
        }

        public string GetDllPath()
        {
            return GetTask().DllPath;
        }
    }
}