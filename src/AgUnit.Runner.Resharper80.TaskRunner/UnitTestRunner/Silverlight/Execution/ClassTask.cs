using AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight.Providers;

namespace AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight.Execution
{
    public class ClassTask
    {
        public TaskNode Node { get; private set; }
        public IClassTaskProvider ClassTaskProvider { get; private set; }

        public ClassTask(TaskNode node, IClassTaskProvider classTaskProvider)
        {
            Node = node;
            ClassTaskProvider = classTaskProvider;
        }

        public string GetFullClassName()
        {
            return ClassTaskProvider.GetFullClassName(Node.Task);
        }
    }
}