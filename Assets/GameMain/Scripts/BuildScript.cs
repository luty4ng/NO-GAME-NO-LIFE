#if UNITY_EDITOR
using System.IO;
using UnityEditor;

public class BuildScript
{
    private static string PromptForPath()
    {
        return EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
    }

    private static void BuildForMac(string path)
    {
        BuildPipeline.BuildPlayer(
            EditorBuildSettings.scenes,
            Path.Combine(path, PlayerSettings.productName),
            BuildTarget.StandaloneOSX,
            BuildOptions.None
        );
    }

    private static void BuildForWindows(string path)
    {
        BuildPipeline.BuildPlayer(
            EditorBuildSettings.scenes,
            Path.Combine(path, PlayerSettings.productName, PlayerSettings.productName + ".exe"),  // folder name followed by executable name
            BuildTarget.StandaloneWindows64,
            BuildOptions.None
        );
    }
    
    [MenuItem("Tools/Build game for Mac")]
    public static void BuildGameForMac()
    {
        var path = PromptForPath();
        if (path.Length == 0) return;
        BuildForMac(path);
    }
    
    [MenuItem("Tools/Build game for Windows")]
    public static void BuildGameForWindows()
    {
        var path = PromptForPath();
        if (path.Length == 0) return;
        BuildForWindows(path);
    }
    
    [MenuItem("Tools/Build game for Mac and Windows")]
    public static void BuildGameForMacAndWindows()
    {
        var path = PromptForPath();
        if (path.Length == 0) return;
        BuildForMac(path);
        BuildForWindows(path);
    }
}
#endif