using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform collectablesParent; // Parent of all mushroom collectibles
    public Camera mainCamera; // Reference to the main camera for visuals
    public RiveTexture riveTexture; // Rive animation component on the player
    public GameObject winScreen; // Win screen UI to show when all mushrooms are collected
    public AudioSource collectSound; // Sound to play on mushroom collection
    public RiveWinScreen riveWinScreen; // Reference to the RiveWinScreen component for the win screen animation

    // Music tracks for different levels
    public AudioSource startMusic;
    public AudioSource oneThirdMusic;
    public AudioSource twoThirdsMusic;
    public AudioSource winMusic;

    private int totalMushrooms;
    private int mushroomsCollected = 0;
    private int oneThirdMushrooms;    // 1/3 of total mushrooms
    private int twoThirdMushrooms;    // 2/3 of total mushrooms
    private bool gameWon = false; // Track if the game has been won
    private float endGameTimer = 0f; // Timer for end game delay
    private float endGameDuration = 15f; // Duration to wait before allowing reset

    private void Start()
    {
        // Calculate total mushrooms and thresholds
        totalMushrooms = collectablesParent.childCount;
        oneThirdMushrooms = Mathf.CeilToInt(totalMushrooms / 3f);
        twoThirdMushrooms = Mathf.CeilToInt(totalMushrooms * 2 / 3f);

        // Set initial character animation level to 1 (default)
        riveTexture.UpdateLevelState(1);

        // Hide the win screen initially
        winScreen.SetActive(false);

        // Start the initial background music
        PlayMusic(startMusic);
    }

    // This function is called whenever a mushroom is collected
    public void CollectMushroom()
    {
        mushroomsCollected++;

        // Play the collection sound when a mushroom is collected
        if (collectSound != null)
        {
            collectSound.PlayOneShot(collectSound.clip);
        }

        UpdateGameState();
    }

// Track the current game phase
    private int currentPhase = 0;

    private void UpdateGameState()
    {
        if (mushroomsCollected >= totalMushrooms)
        {
            WinGame();
        }
        else if (mushroomsCollected >= twoThirdMushrooms && currentPhase < 3)
        {
            ActivateSecondTransition();
            currentPhase = 3;  // Set to 3 for 2/3 phase
        }
        else if (mushroomsCollected >= oneThirdMushrooms && currentPhase < 2)
        {
            ActivateFirstTransition();
            currentPhase = 2;  // Set to 2 for 1/3 phase
        }
    }


    private void ActivateFirstTransition()
    {
        Debug.Log("First transition: Camera visuals activated and character animation level set to 1.");
        riveTexture.UpdateLevelState(2);

        // Play one-third-level music
        PlayMusic(oneThirdMusic);

        // Activate the wobble effect here, if applicable
    }

    private void ActivateSecondTransition()
    {
        Debug.Log("Second transition: More intense visuals and character animation level 2.");
        riveTexture.UpdateLevelState(3);

        // Play two-thirds-level music
        PlayMusic(twoThirdsMusic);
    }

    private void WinGame()
    {
        Debug.Log("Game Won! Activating win screen...");

        // Show the win screen
        winScreen.SetActive(true);

        // Play win music
        PlayMusic(winMusic);

        // Start the Rive win screen animation if assigned
        if (riveWinScreen != null)
        {
            riveWinScreen.StartAnimation();
            Debug.Log("Rive win screen animation started.");
        }
        else
        {
            Debug.LogWarning("RiveWinScreen reference is not assigned in GameManager.");
        }

        // Set game won to true and start the timer
        gameWon = true;
        endGameTimer = 0f;
    }

    private void Update()
    {
        // If the game is won, start counting down
        if (gameWon)
        {
            endGameTimer += Time.deltaTime;

            // Check for any key press after the timer duration has passed
            if (endGameTimer >= endGameDuration && Input.anyKeyDown)
            {
                ResetGame();
            }
        }
    }

    private void ResetGame()
    {
        // Reload the current scene to reset the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Function to stop any currently playing music and start the new one
    private void PlayMusic(AudioSource newMusic)
    {
        // Stop any existing music
        if (startMusic.isPlaying) startMusic.Stop();
        if (oneThirdMusic.isPlaying) oneThirdMusic.Stop();
        if (twoThirdsMusic.isPlaying) twoThirdsMusic.Stop();
        if (winMusic.isPlaying) winMusic.Stop();

        // Play the new music
        if (newMusic != null)
        {
            newMusic.Play();
        }
    }
}
