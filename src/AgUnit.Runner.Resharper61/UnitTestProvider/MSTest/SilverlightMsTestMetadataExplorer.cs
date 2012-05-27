extern alias mstestlegacy;
extern alias mstest10;
#if RS70
extern alias mstest11;
#endif
using System;
using System.Linq;
using AgUnit.Runner.Resharper61.Util;
using JetBrains.Application;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Utils;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper61.UnitTestProvider.MSTest
{
    [MetadataUnitTestExplorer]
    public class SilverlightMsTestMetadataExplorer : IUnitTestMetadataExplorer
    {
        private const string DotNetMsTestAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTestFramework";
        private const string SilverlightMsTestAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTesting.Silverlight";

        private readonly IShellLocks shellLocks;
        private readonly mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTestLegacyProvider;
        private readonly mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTestLegacyElementFactory;
        private readonly mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTest10Provider;
        private readonly mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTest10ElementFactory;
#if RS70
        private readonly mstest11::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTest11Provider;
        private readonly mstest11::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTest11ElementFactory;
#endif

        public IUnitTestProvider Provider
        {
            get
            {
#if RS70
                if (msTest11Provider as IUnitTestProvider != null)
                    return msTest11Provider as IUnitTestProvider;
#endif

                if (msTest10Provider as IUnitTestProvider != null)
                    return msTest10Provider as IUnitTestProvider;

                return msTestLegacyProvider;
            }
        }

        public SilverlightMsTestMetadataExplorer(IShellLocks shellLocks,
            mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTestLegacyProvider = null,
            mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTestLegacyElementFactory = null,
            mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTest10Provider = null,
            mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTest10ElementFactory = null
#if RS70
,
            mstest11::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTest11Provider = null,
            mstest11::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTest11ElementFactory = null
#endif
)
        {
            this.shellLocks = shellLocks;
            this.msTestLegacyProvider = msTestLegacyProvider;
            this.msTestLegacyElementFactory = msTestLegacyElementFactory;
            this.msTest10Provider = msTest10Provider;
            this.msTest10ElementFactory = msTest10ElementFactory;
#if RS70
            this.msTest11Provider = msTest11Provider;
            this.msTest11ElementFactory = msTest11ElementFactory;
#endif
        }

        public void ExploreAssembly(IProject project, IMetadataAssembly assembly, UnitTestElementConsumer consumer)
        {
            if (assembly.ReferencedAssembliesNames.Any(n => n.Name == SilverlightMsTestAssemblyName))
            {
                var originalReferencedAssemblies = InjectMsTestIntoReferencedAssemblyNames(assembly);

                try
                {
#if RS70
                    if (msTest11Provider != null)
                    {
                        new mstest11::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestMetadataExplorer(msTest11ElementFactory, project, shellLocks, consumer).ExploreAssembly(assembly);
                    }
                    else
#endif
                    if (msTest10Provider != null)
                    {
                        new mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestMetadataExplorer(msTest10ElementFactory, project, shellLocks, consumer).ExploreAssembly(assembly);
                    }
                    else if (msTestLegacyProvider != null)
                    {
                        new mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestMetadataExplorer(msTestLegacyElementFactory, project, shellLocks, consumer).ExploreAssembly(assembly);
                    }
                }
                finally
                {
                    SetReferencedAssemblyNames(assembly, originalReferencedAssemblies);
                }
            }
        }

        private AssemblyNameInfo[] InjectMsTestIntoReferencedAssemblyNames(IMetadataAssembly assembly)
        {
            var referencedAssemblyNames = assembly.ReferencedAssembliesNames.ToArray();
            var updatedReferencedAssemblyNames = new AssemblyNameInfo[referencedAssemblyNames.Length + 1];

            Array.Copy(referencedAssemblyNames, updatedReferencedAssemblyNames, referencedAssemblyNames.Length);
            updatedReferencedAssemblyNames[referencedAssemblyNames.Length] = new AssemblyNameInfo(DotNetMsTestAssemblyName);

            SetReferencedAssemblyNames(assembly, updatedReferencedAssemblyNames);

            return referencedAssemblyNames;
        }

        private void SetReferencedAssemblyNames(IMetadataAssembly assembly, AssemblyNameInfo[] referencedAssemblyNames)
        {
            assembly.SetField("myReferencedAssembliesNames", referencedAssemblyNames);
        }
    }
}