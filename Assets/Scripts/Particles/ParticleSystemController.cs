using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    #region Variables
    // Reference to the current Particle System component used for failure effects.
    private ParticleSystem currentFailureEffect;
    #endregion

    #region Initialization
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Initialize the currentFailureEffect with the ParticleSystem component attached to this GameObject.
        currentFailureEffect = GetComponent<ParticleSystem>();
    }
    #endregion

    #region Particle Effects
    // Public method to play the failure effect particle system.
    public void PlayFailParticleEffect()
    {
        // Check if the currentFailureEffect is properly assigned.
        if (currentFailureEffect != null)
        {
            // Play the particle system if it exists.
            currentFailureEffect.Play();
        }
        else
        {
            // Log an error if no particle system is found. This helps in debugging.
            Debug.LogError("Attempted to play a non-existent Particle System.");
        }
    }
    #endregion
}
