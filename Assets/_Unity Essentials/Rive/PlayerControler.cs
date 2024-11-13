using UnityEngine;

// Controls player movement and rotation.
public class PlayerControler : MonoBehaviour
{
    public float speed = 5.0f; // Set player's movement speed.
    public float rotationSpeed = 120.0f; // Set player's rotation speed.

    public AudioSource forwardSound; // Sound for moving forward
    public AudioSource turnSound; // Sound for turning or reversing

    private Rigidbody rb; // Reference to player's Rigidbody.
    private int direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Access player's Rigidbody.
    }

    private void FixedUpdate()
    {
        // Move player based on vertical input.
        float moveVertical = Input.GetAxis("Vertical");
        float adjustedSpeed = moveVertical < 0 ? speed * 0.1f : speed; // 10% speed for reverse
        Vector3 movement = transform.forward * moveVertical * adjustedSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Rotate player based on horizontal input.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float turn = moveHorizontal * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        // Update Rive animation based on movement direction
        UpdateRiveAnimation(moveVertical, moveHorizontal);

        // Play audio based on movement direction
        UpdateAudio(moveVertical, moveHorizontal);
    }

    // Determine direction and update Rive animation
    private void UpdateRiveAnimation(float vertical, float horizontal)
    {
        RiveTexture riveTexture = GetComponentInChildren<RiveTexture>();
        if (riveTexture == null)
        {
            Debug.LogWarning("RiveTexture component not found on child object.");
            return;
        }

        direction = 0; // Default to idle

        if (vertical > 0) // Moving forward
        {
            direction = 1;
        }
        else if (vertical < 0) // Moving backward
        {
            direction = 3;
        }
        else if (horizontal > 0) // Turning right
        {
            direction = 4;
        }
        else if (horizontal < 0) // Turning left
        {
            direction = 2;
        }

        riveTexture.UpdateDirectionInput(direction);
    }

    // Play or stop sounds based on movement direction
    private void UpdateAudio(float vertical, float horizontal)
    {
        if (direction == 1) // Moving forward
        {
            if (!forwardSound.isPlaying)
            {
                forwardSound.loop = true;
                forwardSound.Play();
            }

            if (turnSound.isPlaying)
            {
                turnSound.Stop();
            }
        }
        else if (direction == 2 || direction == 3 || direction == 4) // Turning or reversing
        {
            if (!turnSound.isPlaying)
            {
                turnSound.loop = true;
                turnSound.Play();
            }

            if (forwardSound.isPlaying)
            {
                forwardSound.Stop();
            }
        }
        else // Idle
        {
            if (forwardSound.isPlaying)
            {
                forwardSound.Stop();
            }
            if (turnSound.isPlaying)
            {
                turnSound.Stop();
            }
        }
    }
}
