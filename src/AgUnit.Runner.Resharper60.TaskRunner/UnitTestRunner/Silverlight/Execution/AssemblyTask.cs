using System;
using AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Providers;

namespace AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Execution
{
    public class AssemblyTask
    {
        public TaskNode Node { get; private set; }
        public IAssemblyTaskProvider AssemblyTaskProvider { get; private set; }

        public AssemblyTask(TaskNode node, IAssemblyTaskProvider assemblyTaskProvider)
        {
            Node = node;
            AssemblyTaskProvider = assemblyTaskProvider;
        }

        public void Execute(Action<AssemblyTask> execute)
        {
            Node.Execute(execute, this);
        }

        public string GetXapPath()
        {
            return AssemblyTaskProvider.GetXapPath(Node.Task);
        }
    }
}