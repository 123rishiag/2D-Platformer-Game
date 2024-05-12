using UnityEngine;

// Controls the health display and animations for the player health hearts.
public class PlayerHealthController : MonoBehaviour
{
    #region Variables
    private Animator animator; // Reference to the animator component.
    private int lastHealth; // The health value during the last frame.
    private int currentHealth; // The current health value of the player.
    private int heartIndex; // Index of this heart in the UI.

    public PlayerController playerController; // Reference to the player controller to access player health.
    #endregion

    #region Unity Lifecycle Functions
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Initialize the animator component.
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update.
    private void Start()
    {
        // Retrieve the initial health of the player to set up the lastHealth.
        lastHealth = playerController.GetHealth();
    }

    // Update is called once per frame.
    private void Update()
    {
        // Determine the UI heart's index relative to its siblings.
        heartIndex = transform.GetSiblingIndex();

        // Update the current health from the player controller.
        currentHealth = playerController.GetHealth();

        // Check if this heart represents the current health level and if the health has decreased.
        if (currentHealth == heartIndex && currentHealth < lastHealth)
        {
            // Trigger the health lost animation.
            HealthLostTrigger();
        }

        // Update lastHealth for the next frame.
        lastHealth = playerController.GetHealth();
    }
    #endregion

    #region Health Change Triggers
    // Trigger for when health is lost.
    private void HealthLostTrigger()
    {
        // Trigger the 'healthLost' animation.
        animator.SetTrigger("healthLost");
    }

    // Trigger for when health is completely lost.
    private void HealthEmptyTrigger()
    {
        // Trigger the 'healthEmpty' animation.
        animator.SetTrigger("healthEmpty");
    }
    #endregion
}
