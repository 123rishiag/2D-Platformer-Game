using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    private ParticleSystem currentFailureEffect;

    private void Awake()
    {
        currentFailureEffect = GetComponent<ParticleSystem>();
    }

    public void PlayFailParticleEffect()
    {
        if (currentFailureEffect != null)
        {
            currentFailureEffect.Play();
        }
        else
        {
            Debug.LogError("Attempted to play a non-existent Particle System.");
        }
    }
}
