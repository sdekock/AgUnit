using System;
using JetBrains.ProjectModel;
using JetBrains.VsIntegration.ProjectModel;

namespace AgUnit.Runner.Resharper80.UnitTestFramework.SilverlightPlatform
{
    public static class ProjectExtensions
    {
        public static string GetXapPath(this IProject silverlightProject)
        {
            try
            {
                var projectModelSynchronizer = silverlightProject.GetSolution().GetComponent<ProjectModelSynchronizer>();
                var vsProjectInfo = projectModelSynchronizer.GetProjectInfoByProject(silverlightProject);

                if (vsProjectInfo != null)
                {
                    var project = vsProjectInfo.GetExtProject();
                    var xapOutputs = (bool)project.Properties.Item("SilverlightProject.XapOutputs").Value;

                    if (xapOutputs)
                    {
                        var xapFileName = (string)project.Properties.Item("SilverlightProject.XapFilename").Value;
                        return silverlightProject.GetOutputDirectory().Combine(xapFileName).FullPath;
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
            return silverlightProject.GetOutputFilePath().FullPath;
        }
    }
}