extern alias util;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper60.UnitTestFramework
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

        public ITaskRunnerHostController CreateHostController(UnitTestManager manager, UnitTestSessionManager sessionManager, IUnitTestLaunch launch, string remotingAddress)
        {
            launch.EnsureSilverlightPlatformSupport(manager);

            return CreateWrappedHostController(manager, sessionManager, launch, remotingAddress);
        }

        protected virtual ITaskRunnerHostController CreateWrappedHostController(UnitTestManager manager, UnitTestSessionManager sessionManager, IUnitTestLaunch launch, string remotingAddress)
        {
            return WrappedHostProvider.CreateHostController(manager, sessionManager, launch, remotingAddress);
        }
    }
}   