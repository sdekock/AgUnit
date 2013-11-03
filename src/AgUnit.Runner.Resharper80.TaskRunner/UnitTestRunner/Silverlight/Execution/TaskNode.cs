using System;
using System.Linq;
using JetBrains.ReSharper.TaskRunnerFramework;
using StatLight.Core.Events;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Execution
{
    public enum TaskStatus
    {
        NotStarted,
        Running,
        Finished
    }

    public class TaskNode
    {
        public RemoteTask Task { get; private set; }
        public TaskEnvironment Environment { get; private set; }
        public TaskNode Parent { get; private set; }
        public TaskNode[] Children { get; private set; }
        public TaskStatus Status { get; private set; }
        public TaskResult Result { get; private set; }

        public TaskNode(TaskExecutionNode node, TaskEnvironment environment, TaskNode parent = null)
        {
            Task = node.RemoteTask;
            Environment = environment;
            Parent = parent;
            Status = TaskStatus.NotStarted;
            Result = TaskResult.Skipped;
            Children = node.Children.Select(child => new TaskNode(child, environment, this)).ToArray();
        }

        public void Execute<T>(Action<T> execute, T task)
        {
            try
            {
                NotifyStarting();
                execute(task);
                NotifyFinished(null, TaskResult.Success);
            }
            catch (Exception e)
            {
                NotifyFinishedWithException(e);
                throw;
            }
        }

        public void NotifyStarting()
        {
            if (Status == TaskStatus.NotStarted)
            {
                if (Parent != null)
                {
                    Parent.NotifyStarting();
                }

                Status = TaskStatus.Running;

                Environment.Server.TaskStarting(Task);                
            }
        }

        public void NotifyFinished(string output = null, TaskResult? result = null)
        {
            NotifyStarting();

            if (Status == TaskStatus.Running)
            {
                Status = TaskStatus.Finished;
                Result = result ?? (Children.Any() ? Children.Max(c => c.Result) : TaskResult.Success);

                foreach (var child in Children)
                {
                    child.NotifyFinished(null, TaskResult.Skipped);
                }

                Environment.Server.TaskFinished(Task, output, Result);

                if (Parent != null && Parent.Children.All(c => c.Status == TaskStatus.Finished))
                {
                    Parent.NotifyFinished(null);
                }
            }
        }

        public void NotifyFinishedWithException(Exception e, TaskResult result = TaskResult.Exception)
        {
            NotifyStarting();

            Environment.Server.TaskException(Task, new[] { new TaskException(e) });

            NotifyFinished(e.Message, result);
        }

        public void NotifyFinishedWithException(ExceptionInfo e, TaskResult result = TaskResult.Exception)
        {
            NotifyStarting();

            // TODO: We should pass the TypeName instead of FullMessage
            // TODO: Report inner exceptions recursively
            Environment.Server.TaskException(Task, new[] { new TaskException(e.FullMessage, e.Message, e.StackTrace) });

            NotifyFinished(e.Message, result);
        }
    }
}