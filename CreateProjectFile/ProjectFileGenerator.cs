using Microsoft.Build.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCsprojFileLib
{
    public class ProjectFileGenerator
    {
        public static void CreateProjectFile(string projectName, string configuration, string platform, string[] listReferences, string[] listClasses)
        {

            var root = ProjectRootElement.Create();
            var group = root.AddPropertyGroup();
            group.AddProperty("Configuration", configuration); //Debug or Release
            group.AddProperty("Platform", platform); //x86 or x64 AnyCPU
            group.AddProperty("TargetFrameworkVersion", "v4.5.2");
            group.AddProperty("FileAlignment", "512");
            group.AddProperty("AutoGenerateBindingRedirects", "true");
            root.AddImport(@"$(MSBuildToolsPath)\Microsoft.CSharp.targets");

            var group2 = root.AddPropertyGroup();
            group2.Condition = " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ";
            group2.AddProperty("PlatformTarget", "AnyCPU");
            group2.AddProperty("DebugSymbols", "true");
            group2.AddProperty("DebugType", "full");
            group2.AddProperty("Optimize", "false");
            group2.AddProperty("OutputPath", @"bin\Debug\");
            group2.AddProperty("DefineConstants", "DEBUG;TRACE");
            group2.AddProperty("ErrorReport", "prompt");
            group2.AddProperty("WarningLevel", "4");

            var group3 = root.AddPropertyGroup();
            group3.Condition = " '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ";
            group3.AddProperty("PlatformTarget", "AnyCPU");
            group3.AddProperty("DebugType", "pdbonly");
            group3.AddProperty("Optimize", "true");
            group3.AddProperty("OutputPath", @"bin\Release\");
            group3.AddProperty("DefineConstants", "TRACE");
            group3.AddProperty("ErrorReport", "prompt");
            group3.AddProperty("WarningLevel", "4");


            // references
            AddItems(root, "Reference", listReferences);

            // items to compile
            AddItems(root, "Compile", listClasses);

            AddItemsWithLocation(root, "Reference", new string[] { "Newtonsoft.Json", "RestSharp" });

            var target = root.AddTarget("Build");
            var task = target.AddTask("Csc");
            task.SetParameter("Sources", "@(Compile)");
            task.SetParameter("OutputAssembly", projectName + ".dll");




            root.Save(projectName + ".csproj");
        }

        private static void AddItems(ProjectRootElement elem, string groupName, params string[] items)
        {
            var group = elem.AddItemGroup();
            foreach (var item in items)
            {
                group.AddItem(groupName, item);
            }
        }


        private static void AddItemsWithLocation(ProjectRootElement elem, string groupName, params string[] items)
        {
            var group = elem.AddItemGroup();
            foreach (var item in items)
            {
                ProjectItemElement element = group.AddItem(groupName, item);
                element.AddMetadata("HintPath", @"C:\" + item + ".dll");

            }
        }
    }
}
