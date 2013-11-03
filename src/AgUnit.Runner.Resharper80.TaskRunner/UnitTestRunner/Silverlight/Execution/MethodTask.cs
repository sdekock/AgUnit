using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Execution
{
    public class MethodTask
    {
        public TaskNode Node { get; private set; }
        public IMethodTaskProvider MethodTaskProvider { get; private set; }

        public MethodTask(TaskNode node, IMethodTaskProvider methodTaskProvider)
        {
            Node = node;
            MethodTaskProvider = methodTaskProvider;
        }

        public string GetFullMethodName()
        {
            return MethodTaskProvider.GetFullMethodName(Node.Task);
        }
    }
}