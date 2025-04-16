using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class ScriptRenamer : EditorWindow
{
    private string oldScriptName = "";
    private string newScriptName = "";

    [MenuItem("Tools/Script Renamer")]
    public static void ShowWindow()
    {
        GetWindow<ScriptRenamer>("Script Renamer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Rename Script and Update References", EditorStyles.boldLabel);

        oldScriptName = EditorGUILayout.TextField("Old Script Name", oldScriptName);
        newScriptName = EditorGUILayout.TextField("New Script Name", newScriptName);

        if (GUILayout.Button("Rename Script"))
        {
            if (string.IsNullOrEmpty(oldScriptName) || string.IsNullOrEmpty(newScriptName))
            {
                Debug.LogError("Both old and new script names must be provided!");
                return;
            }
            RenameScript(oldScriptName, newScriptName);
        }
    }

    private void RenameScript(string oldName, string newName)
    {
        // Get the path of the old script file
        string oldScriptPath = FindScriptPath(oldName);
        if (string.IsNullOrEmpty(oldScriptPath))
        {
            Debug.LogError("Script not found!");
            return;
        }

        // Find all references in the project
        string[] allScripts = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
        foreach (var scriptPath in allScripts)
        {
            string scriptContent = File.ReadAllText(scriptPath);
            if (scriptContent.Contains(oldName))
            {
                // Replace the old script name with the new one
                string updatedContent = Regex.Replace(scriptContent, @"\b" + oldName + @"\b", newName);
                File.WriteAllText(scriptPath, updatedContent);
                Debug.Log($"Updated references in: {scriptPath}");
            }
        }

        // Rename the script file
        string newScriptPath = Path.Combine(Path.GetDirectoryName(oldScriptPath), newName + ".cs");
        AssetDatabase.MoveAsset(oldScriptPath, newScriptPath);
        AssetDatabase.Refresh();

        Debug.Log($"Script renamed from {oldName}.cs to {newName}.cs");
    }

    private string FindScriptPath(string scriptName)
    {
        string[] allScripts = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
        foreach (var scriptPath in allScripts)
        {
            if (scriptPath.EndsWith(scriptName + ".cs"))
            {
                return scriptPath;
            }
        }
        return null;
    }
}
