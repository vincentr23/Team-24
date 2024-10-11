using UnityEngine;

public class HeartbeatPulse : MonoBehaviour
{
    public float pulseSpeed = 2.0f; // Speed of the heartbeat pulse
    public float scaleMultiplier = 1.1f; // How much to scale the heart
    public float minFogDensity = 0.02f; // Minimum fog density
    public float maxFogDensity = 0.1f; // Maximum fog density

    private Vector3 originalScale; // To store the original scale of the heart

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale of the heart
    }

    void Update()
    {
        // Create a pulsing effect using sine wave
        float scale = Mathf.Sin(Time.time * pulseSpeed) * (scaleMultiplier - 1) + 1;

        // Set the heart's scale while keeping the local position of child objects unchanged
        transform.localScale = new Vector3(originalScale.x * scale, originalScale.y * scale, originalScale.z * scale);

        // Reset child scales to avoid scaling issues
        foreach (Transform child in transform)
        {
            child.localScale = Vector3.one; // Reset child scale to one
        }

        // Adjust fog density based on heartbeat pulse
        float fogDensity = Mathf.Lerp(minFogDensity, maxFogDensity, (scale - 1) / (scaleMultiplier - 1));
        RenderSettings.fogDensity = fogDensity; // Apply the fog density
    }
}
