using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ChemistryLab.Desktop.Editor
{
    /// <summary>
    /// Development Android APK build entry for the canonical Chemistry Lab Unity project.
    /// It intentionally uses Unity's debug keystore and produces an installable APK rather
    /// than a Play Store AAB. The existing runtime touch controls are enabled automatically
    /// by Application.isMobilePlatform on device.
    /// </summary>
    public static class AndroidLabBuild
    {
        private const string ScenePath = "Assets/ChemistryLab/Scenes/DesktopChemistryLab.unity";
        private const string OutputDirectory = "Builds/Android";
        private const string OutputFileName = "ChemistryLab3D-Android-dev.apk";
        private const string ApplicationId = "com.chemistrylab.simulator";

        [MenuItem("Chemistry Lab/Android/Build Development APK")]
        public static void BuildDevelopmentApk()
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Android, BuildTarget.Android))
            {
                throw new InvalidOperationException(
                    "Android Build Support is not installed for this Unity Editor. "
                    + "Install Android Build Support, Android SDK & NDK Tools, and OpenJDK from Unity Hub.");
            }

            // Re-run the project's normal validation and scene generation before building.
            DesktopLabBuild.CreateScene();

            if (!EditorUserBuildSettings.SwitchActiveBuildTarget(
                    NamedBuildTarget.Android,
                    BuildTarget.Android))
            {
                throw new InvalidOperationException(
                    "Unity could not switch to the Android build target. Check the installed Android module and Editor license.");
            }

            ConfigureAndroidPlayerSettings();

            var projectRoot = Directory.GetParent(Application.dataPath).FullName;
            var outputDirectory = Path.Combine(projectRoot, OutputDirectory);
            Directory.CreateDirectory(outputDirectory);
            var apkPath = Path.Combine(outputDirectory, OutputFileName);

            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            EditorUserBuildSettings.buildAppBundle = false;

            var options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = apkPath,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                options = BuildOptions.Development | BuildOptions.StrictMode
            };

            var report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Android APK build failed: " + report.summary.result
                    + " · errors=" + report.summary.totalErrors
                    + " · warnings=" + report.summary.totalWarnings);
            }

            Debug.Log(
                "ANDROID_LAB_BUILD_PASS path=" + apkPath
                + " size=" + report.summary.totalSize
                + " warnings=" + report.summary.totalWarnings);
        }

        private static void ConfigureAndroidPlayerSettings()
        {
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, ApplicationId);
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

            PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
        }
    }
}
