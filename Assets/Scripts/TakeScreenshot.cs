using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

// This is the component you attach to a GameObject.
public class TakeScreenshot : MonoBehaviour
{
    public void TakeMarketingScreenshot()
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = $"MarketingShot_{timestamp}.png";
        string filePath = Path.Combine(Application.dataPath, fileName);

        ScreenCapture.CaptureScreenshot(filePath);

        Debug.Log($"<color=cyan>Screenshot saved to:</color> {filePath}");

#if UNITY_EDITOR
        // This makes the file appear in your Project window instantly.
        AssetDatabase.Refresh();

        // This highlights the new file in the Project window.
        Object newScreenshot = AssetDatabase.LoadAssetAtPath($"Assets/{fileName}", typeof(Texture2D));
        EditorGUIUtility.PingObject(newScreenshot);
#endif
    }
}

#if UNITY_EDITOR
// The editor class name is updated for consistency.
[CustomEditor(typeof(TakeScreenshot))]
public class TakeScreenshotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TakeScreenshot myScript = (TakeScreenshot)target;

        EditorGUILayout.Space(10);

        // --- Capture Button ---
        // Save the original GUI color before changing it.
        Color originalColor = GUI.backgroundColor;
        try
        {
            // Set the desired color.
            GUI.backgroundColor = new Color(0.7f, 1f, 0.7f);
            if (GUILayout.Button("Capture Screenshot", GUILayout.Height(40)))
            {
                myScript.TakeMarketingScreenshot();
            }
        }
        finally
        {
            // IMPORTANT: Always restore the original color.
            GUI.backgroundColor = originalColor;
        }

        // --- Open Folder Button ---
        if (GUILayout.Button("Open Save Folder", GUILayout.Height(25)))
        {
            // Use the more appropriate method for opening a folder in the editor.
            EditorUtility.RevealInFinder(Application.dataPath);
        }
    }
}
#endif