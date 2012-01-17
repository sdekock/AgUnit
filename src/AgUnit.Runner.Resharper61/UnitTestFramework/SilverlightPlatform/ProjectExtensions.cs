﻿using System;
using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.VsIntegration.ProjectModel;

namespace AgUnit.Runner.Resharper60.UnitTestFramework.SilverlightPlatform
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

                        return silverlightProject.ActiveConfiguration.OutputDirectory.Combine(xapFileName).FullPath;
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