using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("It exists");
            currentFailureEffect.Play();
        }
        else
        {
            Debug.LogError("Attempted to play a non-existent Particle System.");
        }
    }
}
