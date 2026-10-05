using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace ChemistryLab.BuildAutomation
{
    public static class AndroidCloudBuild
    {
        public static void PreExport()
        {
            PlayerSettings.SetApplicationIdentifier(
                NamedBuildTarget.Android,
                "com.chemistrylab.simulator");

            PlayerSettings.SetScriptingBackend(
                NamedBuildTarget.Android,
                ScriptingImplementation.IL2CPP);

            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            PlayerSettings.bundleVersion = "0.3.1";
            PlayerSettings.Android.bundleVersionCode = 4;

            PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;

            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            EditorUserBuildSettings.buildAppBundle = false;

            Debug.Log(
                "ANDROID_CLOUD_PREEXPORT_PASS backend=IL2CPP arch=ARM64 minApi=26 version=0.3.1 output=APK");
        }
    }
}
