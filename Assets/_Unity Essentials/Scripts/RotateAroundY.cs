using UnityEngine;

public class RotateAroundY : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation in degrees per second

    private void Update()
    {
        // Rotate the object around its local Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }
}
