using System;
using System.Collections.Generic;
using System.Linq;
using AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight.Execution;
using JetBrains.ReSharper.TaskRunnerFramework;
using StatLight.Core.Events;

namespace AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight
{
    public class SilverlightResultsHandler : ITestingReportEvents,
        IListener<TestExecutionClassBeginClientEvent>,
        IListener<TestExecutionClassCompletedClientEvent>,
        IListener<TestExecutionMethodBeginClientEvent>
    {
        public IEnumerable<ClassTask> TestClasses { get; private set; }
        public IEnumerable<MethodTask> TestMethods { get; private set; }

        public SilverlightResultsHandler(IEnumerable<ClassTask> testClasses, IEnumerable<MethodTask> testMethods)
        {
            TestClasses = testClasses;
            TestMethods = testMethods;
        }

        public void Handle(TestCaseResultServerEvent message)
        {
            if (message.MethodName != null)
            {
                var testMethod = GetTestMethod(message.FullMethodName());
                if (testMethod != null)
                {
                    var taskResult = ToTaskResult(message.ResultType);

                    if (message.ExceptionInfo != null)
                    {
                        testMethod.Node.NotifyFinishedWithException(message.ExceptionInfo, taskResult);
                    }
                    else
                    {
                        testMethod.Node.NotifyFinished(null, taskResult);
                    }
                }
            }
        }

        private TaskResult ToTaskResult(ResultType resultType)
        {
            switch (resultType)
            {
                case ResultType.Passed:
                    return TaskResult.Success;
                case ResultType.Failed:
                    return TaskResult.Exception;
                case ResultType.Ignored:
                    return TaskResult.Skipped;
                case ResultType.SystemGeneratedFailure:
                    return TaskResult.Error;
            }

            throw new ArgumentOutOfRangeException();
        }

        public void Handle(TraceClientEvent message)
        { }

        public void Handle(BrowserHostCommunicationTimeoutServerEvent message)
        {
            throw new Exception(message.Message);
        }

        public void Handle(FatalSilverlightExceptionServerEvent message)
        {
            throw new Exception(message.Message);
        }

        public void Handle(UnhandledExceptionClientEvent message)
        {
            throw new Exception(message.ExceptionInfo.FullMessage);
        }

        public void Handle(TestExecutionClassBeginClientEvent message)
        {
            var testClass = GetTestClass(message);
            if (testClass != null)
            {
                testClass.Node.NotifyStarting();
            }
        }

        public void Handle(TestExecutionClassCompletedClientEvent message)
        {
            var testClass = GetTestClass(message);
            if (testClass != null)
            {
                testClass.Node.NotifyFinished();
            }
        }

        public void Handle(TestExecutionMethodBeginClientEvent message)
        {
            // TODO: For some reason this is triggered AFTER the method is run, so the test timings are incorrect
            var testMethod = GetTestMethod(message.FullMethodName);
            if (testMethod != null)
            {
                testMethod.Node.NotifyStarting();
            }
        }

        private ClassTask GetTestClass(TestExecutionClassClientEvent message)
        {
            var fullClassName = string.Format("{0}.{1}", message.NamespaceName, message.ClassName);
            return TestClasses.FirstOrDefault(c => c.GetFullClassName() == fullClassName);
        }

        private MethodTask GetTestMethod(string fullMethodName)
        {
            return TestMethods.FirstOrDefault(c => c.GetFullMethodName() == fullMethodName);
        }
    }
}