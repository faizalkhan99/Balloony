using UnityEngine;
using TMPro;

public class PerformanceHUD : MonoBehaviour
{
    [Header("Build")]
    [Tooltip("If false, HUD will be removed from non-editor builds.")]
    public bool includeInBuild = true;

    [Header("UI")]
    public TMP_Text text;

    [Header("FPS")]
    [Tooltip("How often (seconds) the HUD refreshes.")] 
    public float refreshRate = 1f;

    private int frameCount;
    private float timeAccum;
    private float fps;
    private float frameTimeMs;
    void Start()
    {
        Application.targetFrameRate = 60;
    }
    void Update()
    {
        frameCount++;
        timeAccum += Time.unscaledDeltaTime;

        if (timeAccum >= refreshRate)
        {
            fps = frameCount / timeAccum;
            frameTimeMs = 1000f / Mathf.Max(fps, 0.01f);

            frameCount = 0;
            timeAccum = 0f;

            UpdateHUD();
        }
    }

    void UpdateHUD()
    {
        if (!text) return;

        bool gpuBound = frameTimeMs > 17f && Time.deltaTime < 0.015f;
        string bottleneck = gpuBound ? "GPU-bound" : "CPU-bound";

        text.text =
$@"FPS: {fps:0}
Frame Time: {frameTimeMs:0.0} ms

Device Resolution: {Screen.width} x {Screen.height}
DPI: {Screen.dpi:0}

Target FPS: {Application.targetFrameRate}
VSync: {(QualitySettings.vSyncCount > 0 ? "ON" : "OFF")}
Quality: {QualitySettings.names[QualitySettings.GetQualityLevel()]}

Post Processing: {(IsPostProcessingEnabled() ? "ON" : "OFF")}

Likely Bottleneck: {bottleneck}

Device:
{SystemInfo.deviceModel}
GPU: {SystemInfo.graphicsDeviceName}";

        text.color = GetFPSColor(fps);
    }

    Color GetFPSColor(float fps)
    {
        if (fps >= 50f) return Color.green;
        if (fps >= 30f) return Color.yellow;
        return Color.red;
    }

    /// <summary>
    /// Built-in Render Pipeline–safe post-processing detection.
    /// Uses reflection so this script compiles even if PP is not installed.
    /// </summary>
    bool IsPostProcessingEnabled()
    {
        // Post Processing Stack v2 type name
        var type = System.Type.GetType(
            "UnityEngine.PostProcessing.PostProcessLayer, Unity.Postprocessing.Runtime"
        );

        if (type == null)
            return false;

        var layers = Object.FindObjectsByType(type, FindObjectsSortMode.None);

        foreach (var layer in layers)
        {
            if (layer is Behaviour b && b.enabled)
                return true;
        }

        return false;
    }
}
