using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using AppleAuth.Editor;

public static class PostProcessBuild
{
#if UNITY_IOS
    public static class PostProcessBuildUtils
    {
        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget != BuildTarget.iOS)
            {
                return;
            }
 
            AddImagesXcAssetsToBuildPhases(path);
            AddSignInWithAppleCapability(path);
        }
 
        private static void AddImagesXcAssetsToBuildPhases(string path)
        {
            string projectPath = PBXProject.GetPBXProjectPath(path);
            var project = new PBXProject();
            project.ReadFromFile(projectPath);
         
            string mainGuid = project.GetUnityMainTargetGuid();
            project.AddFileToBuild
                (mainGuid, project.AddFile
                    ("Unity-iPhone/Images.xcassets", "Images.xcassets"));
            project.WriteToFile(projectPath);
        }
        
        private static void AddSignInWithAppleCapability(string path)
        {
            var projectPath = PBXProject.GetPBXProjectPath(path);
        
            // Adds entitlement depending on the Unity version used
#if UNITY_2019_3_OR_NEWER
            var project = new PBXProject();
            project.ReadFromString(System.IO.File.ReadAllText(projectPath));
            var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", null, project.GetUnityMainTargetGuid());
            manager.AddSignInWithAppleWithCompatibility(project.GetUnityFrameworkTargetGuid());
            manager.WriteToFile();
#else
            var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", PBXProject.GetUnityTargetName());
            manager.AddSignInWithAppleWithCompatibility();
            manager.WriteToFile();
#endif
        }
    }
#endif
}