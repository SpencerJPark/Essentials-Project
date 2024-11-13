using UnityEngine;

public class Floating : MonoBehaviour
{
    public float floatAmplitude = 0.5f;  // How far the object moves up and down
    public float floatSpeed = 1.0f;      // Speed of the floating motion

    private Vector3 startPosition;       // Starting position of the object

    private void Start()
    {
        startPosition = transform.position;  // Record the starting position
    }

    private void Update()
    {
        // Calculate the new Y position using a sine wave for smooth up/down motion
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
