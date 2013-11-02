extern alias util;
using AgUnit.Runner.Resharper61.UnitTestFramework.SilverlightPlatform;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper61.UnitTestFramework
{
    public class HostProviderWrapper<TWrappedHostProvider> : IHostProvider
        where TWrappedHostProvider : IHostProvider
    {
        public TWrappedHostProvider WrappedHostProvider { get; private set; }

        public HostProviderWrapper(TWrappedHostProvider wrappedHostProvider)
        {
            WrappedHostProvider = wrappedHostProvider;
        }

        public string ID
        {
            get { return WrappedHostProvider.ID; }
        }

        public IHostProviderDescriptor Descriptor
        {
            get { return WrappedHostProvider.Descriptor; }
        }

        public bool Available()
        {
            return WrappedHostProvider.Available();
        }

        public bool Available(IUnitTestElement element)
        {
            return WrappedHostProvider.Available(element);
        }

#if RS80
        public ITaskRunnerHostController CreateHostController(ISolution solution, IUnitTestLaunchManager launchManager, IUnitTestAgentManager agentManager, IUnitTestLaunch launch)
        {
            var providers = solution.GetComponent<UnitTestProviders>();
            launch.EnsureSilverlightPlatformSupport(providers);

            return CreateWrappedHostController(solution, launchManager, agentManager, launch);
        }

        protected virtual ITaskRunnerHostController CreateWrappedHostController(ISolution solution, IUnitTestLaunchManager launchManager, IUnitTestAgentManager agentManager, IUnitTestLaunch launch)
        {
            return WrappedHostProvider.CreateHostController(solution, launchManager, agentManager, launch);
        }
#else
        public ITaskRunnerHostController CreateHostController(ISolution solution, IUnitTestSessionManager sessionManager, IUnitTestLaunch launch, string remotingAddress)
        {
            var providers = solution.GetComponent<UnitTestProviders>();
            launch.EnsureSilverlightPlatformSupport(providers);

            return CreateWrappedHostController(solution, sessionManager, launch, remotingAddress);
        }

        protected virtual ITaskRunnerHostController CreateWrappedHostController(ISolution solution, IUnitTestSessionManager sessionManager, IUnitTestLaunch launch, string remotingAddress)
        {
            return WrappedHostProvider.CreateHostController(solution, sessionManager, launch, remotingAddress);
        }
#endif
    }
}   