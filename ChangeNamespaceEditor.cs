using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class ChangeNamespaceEditor : EditorWindow
{
    private string oldNamespace = ""; 
    private string newNamespace = ""; 

    [MenuItem("Tools/Change Script Namespace")]
    public static void ShowWindow()
    {
        GetWindow<ChangeNamespaceEditor>("Change Namespace");
    }

    private void OnGUI()
    {
        GUILayout.Label("Change Namespace of All Scripts", EditorStyles.boldLabel);

        oldNamespace = EditorGUILayout.TextField("Old Namespace", oldNamespace);
        newNamespace = EditorGUILayout.TextField("New Namespace", newNamespace);

        if (GUILayout.Button("Change Namespaces"))
        {
            if (string.IsNullOrEmpty(oldNamespace) || string.IsNullOrEmpty(newNamespace))
            {
                Debug.LogError("Please provide both old and new namespaces.");
                return;
            }

            ChangeNamespacesInScripts();
        }
    }

    private void ChangeNamespacesInScripts()
    {
        string[] scriptFiles = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);

        foreach (string file in scriptFiles)
        {
            string fileContents = File.ReadAllText(file);

            if (fileContents.Contains(oldNamespace))
            {
                string updatedContents = Regex.Replace(fileContents, $@"\b{oldNamespace}\b", newNamespace);
                File.WriteAllText(file, updatedContents);
                Debug.Log($"Namespace changed in: {file}");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Namespace change completed for all scripts.");
    }
}
