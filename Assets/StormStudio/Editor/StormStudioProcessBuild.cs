using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class StormStudioProcessBuild : IPostprocessBuildWithReport, IPreprocessBuildWithReport
{

    public int callbackOrder { get { return int.MaxValue; } }

    public void OnPreprocessBuild(BuildReport report)
    {
    }

    public void OnPostprocessBuild(BuildReport report)
    {
#if UNITY_IOS
        string buildPath = report.summary.outputPath;
        string projectPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject project = new PBXProject();
        project.ReadFromString(File.ReadAllText(projectPath));

#if UNITY_2018
        string targetGUID = project.TargetGuidByName("Unity-iPhone");
#else
        string targetGUID = project.GetUnityMainTargetGuid();
#endif

        // sound switch
        var muteFile = Application.dataPath + "/StormStudio/Editor/iOS-Patch/mute.caf";
        File.Copy(muteFile, report.summary.outputPath + "/mute.caf");
        var fileGuid = project.AddFile("mute.caf", "mute.caf");
        project.AddFileToBuild(targetGUID, fileGuid);

#if UNITY_2019_3_OR_NEWER
        // Unity 2019 issue - GoogleService-Info.plist is not added to main target
        string googleInfoPlistGuid = project.FindFileGuidByProjectPath("GoogleService-Info.plist");
        project.AddFileToBuild(targetGUID, googleInfoPlistGuid);
#endif

        File.WriteAllText(projectPath, project.WriteToString());

        // wkweview
        string plistPath = report.summary.outputPath + "/Info.plist";
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));
        PlistElementDict rootDict = plist.root;
        rootDict.SetString("gad_preferred_webview", "wkwebview");
#if USE_MAX_MEDIATION
        rootDict.SetString("NSCalendarsUsageDescription", "Used to deliver better advertising experience");
#endif
        File.WriteAllText(plistPath, plist.WriteToString());
#endif
    }
}
