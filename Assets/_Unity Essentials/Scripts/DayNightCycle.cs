using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Tooltip("Duration of one day in seconds.")]
    public float dayDuration = 120f; // Default to 120 seconds per day (editable in Inspector)

    private float rotationSpeed;

    private void Start()
    {
        // Calculate the rotation speed in degrees per second
        rotationSpeed = 360f / dayDuration;
    }

    private void Update()
    {
        // Rotate the light along the x-axis to simulate day passing
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
