using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AndroidPluginCleaner : EditorWindow
{
    private class PluginFile
    {
        public string path;
        public DateTime lastWriteTime;
        public bool selected;
        public bool suspicious;
    }

    private Vector2 scroll;
    private List<PluginFile> pluginFiles = new();

    private static readonly string[] suspiciousKeywords =
    {
        "googlemobileads",
        "unityads",
        "applovin",
        "ironsource",
        "facebook",
        "chartboost"
    };

    [MenuItem("Tools/Android Plugin Cleaner")]
    public static void ShowWindow()
    {
        GetWindow<AndroidPluginCleaner>("Android Plugin Cleaner");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scan & Clean Android Plugins (.aar / .jar)", EditorStyles.boldLabel);

        if (GUILayout.Button("Scan Project"))
        {
            ScanPlugins();
        }

        if (pluginFiles.Count == 0)
        {
            GUILayout.Label("No scan results yet.");
            return;
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Select All Suspicious"))
        {
            foreach (var file in pluginFiles)
                file.selected = file.suspicious;
        }

        if (GUILayout.Button("Delete Selected"))
        {
            DeleteSelected();
        }

        GUILayout.Space(10);

        scroll = GUILayout.BeginScrollView(scroll);

        foreach (var file in pluginFiles)
        {
            GUILayout.BeginHorizontal();

            file.selected = GUILayout.Toggle(file.selected, "", GUILayout.Width(20));

            GUI.color = file.suspicious ? Color.red : Color.white;

            GUILayout.Label(Path.GetFileName(file.path), GUILayout.Width(250));
            GUILayout.Label(file.lastWriteTime.ToString("yyyy-MM-dd"), GUILayout.Width(120));
            GUILayout.Label(file.path);

            GUI.color = Color.white;

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }

    private void ScanPlugins()
    {
        pluginFiles.Clear();

        string root = Path.Combine(Application.dataPath, "Plugins/Android");

        if (!Directory.Exists(root))
        {
            Debug.LogWarning("No Plugins/Android folder found.");
            return;
        }

        var files = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories)
            .Where(f => f.EndsWith(".aar") || f.EndsWith(".jar"));

        foreach (var file in files)
        {
            var info = new FileInfo(file);

            bool isSuspicious =
                suspiciousKeywords.Any(k => file.ToLower().Contains(k)) ||
                info.LastWriteTime.Year < 2019; // old SDK heuristic

            pluginFiles.Add(new PluginFile
            {
                path = file.Replace(Application.dataPath, "Assets"),
                lastWriteTime = info.LastWriteTime,
                suspicious = isSuspicious,
                selected = false
            });
        }

        Debug.Log($"Scan complete. Found {pluginFiles.Count} plugin files.");
    }

    private void DeleteSelected()
    {
        var toDelete = pluginFiles.Where(f => f.selected).ToList();

        if (toDelete.Count == 0)
        {
            Debug.LogWarning("No files selected.");
            return;
        }

        if (!EditorUtility.DisplayDialog(
            "Confirm Delete",
            $"Delete {toDelete.Count} selected files?",
            "Yes", "Cancel"))
        {
            return;
        }

        foreach (var file in toDelete)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), file.path);

            try
            {
                File.Delete(fullPath);

                string metaPath = fullPath + ".meta";
                if (File.Exists(metaPath))
                    File.Delete(metaPath);

                Debug.Log($"Deleted: {file.path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete {file.path}: {e.Message}");
            }
        }

        AssetDatabase.Refresh();
        ScanPlugins();
    }
}