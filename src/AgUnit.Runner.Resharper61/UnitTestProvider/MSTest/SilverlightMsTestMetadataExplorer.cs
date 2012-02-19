extern alias mstest10;
extern alias mstestlegacy;
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
        private readonly mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTest10Provider;
        private readonly mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTestLegacyProvider;
        private readonly mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTest10ElementFactory;
        private readonly mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTestLegacyElementFactory;

        public IUnitTestProvider Provider
        {
            get { return msTest10Provider as IUnitTestProvider ?? msTestLegacyProvider; }
        }

        public SilverlightMsTestMetadataExplorer(IShellLocks shellLocks, mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTest10Provider = null, mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTestLegacyProvider = null,
            mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTest10ElementFactory = null, mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTestLegacyElementFactory = null)
        {
            this.shellLocks = shellLocks;
            this.msTest10Provider = msTest10Provider;
            this.msTestLegacyProvider = msTestLegacyProvider;
            this.msTest10ElementFactory = msTest10ElementFactory;
            this.msTestLegacyElementFactory = msTestLegacyElementFactory;
        }

        public void ExploreAssembly(IProject project, IMetadataAssembly assembly, UnitTestElementConsumer consumer)
        {
            if (assembly.ReferencedAssembliesNames.Any(n => n.Name == SilverlightMsTestAssemblyName))
            {
                var originalReferencedAssemblies = InjectMsTestIntoReferencedAssemblyNames(assembly);

                try
                {
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
            var referencedAssemblyNames = assembly.ReferencedAssembliesNames;
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