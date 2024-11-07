using UnityEngine;

// Controls player movement and rotation.
public class PlayerControler : MonoBehaviour
{
    public float speed = 5.0f; // Set player's movement speed.
    public float rotationSpeed = 120.0f; // Set player's rotation speed.

    private Rigidbody rb; // Reference to player's Rigidbody.

    private int direction;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Access player's Rigidbody.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Handle physics-based movement and rotation.
private void FixedUpdate()
{
    // Move player based on vertical input.
    float moveVertical = Input.GetAxis("Vertical");

    // Apply a slower speed if moving backward (reverse).
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
}


    // Determine direction and update Rive animation
    // 0 is Idol
    // 1 is forward
    // 2 is left turn
    // 3 is reverse
    // 4 is right turn
    private void UpdateRiveAnimation(float vertical, float horizontal)
    {
        // Reference to the RiveTexture component on the child object
        RiveTexture riveTexture = GetComponentInChildren<RiveTexture>();
            if (riveTexture == null)
            {
                Debug.LogWarning("RiveTexture component not found on child object.");
                return;
            }

        direction = 0; // Default to idol

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

        // Update the animation's direction in the Rive state machine
        riveTexture.UpdateDirectionInput(direction);
    }

}