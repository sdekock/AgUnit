using System;
using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.VsIntegration.ProjectModel;

namespace AgUnit.Runner.Resharper61.UnitTestFramework.SilverlightPlatform
{
    public static class ProjectExtensions
    {
        public static string GetXapPath(this IProject silverlightProject)
        {
            try
            {
                var vsSolutionManager = Shell.Instance.GetComponent<VSSolutionManager>();
                var projectModelSynchronizer = vsSolutionManager.GetProjectModelSynchronizer(silverlightProject.GetSolution());
                var vsProjectInfo = projectModelSynchronizer.GetProjectInfoByProject(silverlightProject);

                if (vsProjectInfo != null)
                {
                    var project = vsProjectInfo.GetExtProject();
                    var xapOutputs = (bool)project.Properties.Item("SilverlightProject.XapOutputs").Value;

                    if (xapOutputs)
                    {
                        var xapFileName = (string)project.Properties.Item("SilverlightProject.XapFilename").Value;

#if RS70
                        return silverlightProject.ActiveConfiguration.GetOutputDirectory(vsProjectInfo.Project.Location).Combine(xapFileName).FullPath;
#else
                        return silverlightProject.ActiveConfiguration.OutputDirectory.Combine(xapFileName).FullPath;
#endif
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetDllPath(this IProject silverlightProject)
        {
            return UnitTestManager.GetOutputAssemblyPath(silverlightProject).FullPath;
        }
    }
}