using UnityEngine;

public class DayNightLightCycle : MonoBehaviour
{
    [Header("Cycle Settings")]
    [Tooltip("How fast the day goes by. Higher numbers make days shorter.")]
    [Range(0.01f, 5f)]
    public float cycleSpeed = 0.1f;

    [Header("Color Grading")]
    [Tooltip("Define the colors for the cycle. Left is morning/noon, middle is evening, right is night.")]
    public Gradient cycleColors;

    private Light[] allLights;
    private float timeTrack = 0f;

    void Start()
    {
        // Find every Light component currently active in the scene
        allLights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);

        if (allLights.Length == 0)
        {
            Debug.LogWarning("DayNightLightCycle: No lights found in the scene!");
        }
    }

    void Update()
    {
        if (allLights == null || allLights.Length == 0) return;

        // Advance time based on your speed setting
        timeTrack += Time.deltaTime * cycleSpeed;

        // Loop the value back to 0 when it hits 1 using PingPong (creates a smooth back-and-forth loop)
        // Alternatively, use Mathf.Repeat(timeTrack, 1f) if you want it to jump straight from night to dawn
        float evaluationTime = Mathf.PingPong(timeTrack, 1f);

        // Evaluate the color from the gradient based on the current time
        Color currentColor = cycleColors.Evaluate(evaluationTime);

        // Apply the new color to every tracked light
        foreach (Light light in allLights)
        {
            if (light != null)
            {
                light.color = currentColor;
            }
        }
    }

    // Optional: Public method if you want to trigger a refresh via UI buttons
    public void RefreshLightList()
    {
        allLights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
    }
}
