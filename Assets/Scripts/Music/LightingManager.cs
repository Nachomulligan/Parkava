using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("Lighting")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField, Range(0f, 24f)] private float timeOfDay;

    [Header("Time")]
    [SerializeField, Min(0.01f), Tooltip("Real-time seconds for one complete 24-hour cycle.")]
    private float cycleDurationSeconds = 24f;

    [Header("Wwise")]
    [SerializeField] private AK.Wwise.RTPC timeRTPC;

    private void Update()
    {
        if (preset == null)
            return;

        if (Application.isPlaying)
        {
            AdvanceTimeOfDay();
            UpdateWwiseTime();
        }

        UpdateLighting(timeOfDay / 24f);
    }

    private void AdvanceTimeOfDay()
    {
        float safeCycleDuration = Mathf.Max(0.01f, cycleDurationSeconds);
        timeOfDay = Mathf.Repeat(timeOfDay + (24f / safeCycleDuration) * Time.deltaTime, 24f);
    }

    private void UpdateWwiseTime()
    {
        if (timeRTPC != null)
            timeRTPC.SetGlobalValue(timeOfDay);
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        if (directionalLight != null)
        {
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(
                (timePercent * 360f) - 90f,
                170f,
                0f);
        }
    }

    private void OnValidate()
    {
        if (directionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
            return;
        }

        Light[] lights = FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            if (light.type == LightType.Directional)
            {
                directionalLight = light;
                break;
            }
        }
    }
}
