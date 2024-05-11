using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    private Animator animator;
    private int lastHealth;
    private int currentHealth;
    private int heartIndex;

    public PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        lastHealth = playerController.GetHealth();
    }

    private void Update()
    {
        heartIndex = transform.GetSiblingIndex();
        currentHealth = playerController.GetHealth();
        if (currentHealth == heartIndex && currentHealth < lastHealth)
        {
            HealthLostTrigger();
        }
        lastHealth = playerController.GetHealth();
    }
    private void HealthLostTrigger()
    {
        animator.SetTrigger("healthLost");
    }
    private void HealthEmptyTrigger()
    {
        animator.SetTrigger("healthEmpty");
    }
}
