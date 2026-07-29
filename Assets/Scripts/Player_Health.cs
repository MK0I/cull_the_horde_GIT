using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;

    [Header("Invulnerability")]
    [SerializeField] private float invulnerabilityTime = 0.75f;

    [Header("VFX")]
    [SerializeField] private ParticleSystem damageEffect;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color damageColor = Color.red;

    private int currentHealth;
    private float invulnerabilityTimer;
    private Color originalColor;

    public int CurrentHealth => currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        // Sync starting HP to HUD
        if (Game_Manager.Instance != null)
            Game_Manager.Instance.SetPlayerHP(currentHealth, maxHealth);
    }

    private void Update()
    {
        if (invulnerabilityTimer > 0f)
        {
            invulnerabilityTimer -= Time.deltaTime;

            if (invulnerabilityTimer <= 0f)
            {
                invulnerabilityTimer = 0f;

                if (spriteRenderer != null)
                    spriteRenderer.color = originalColor;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (invulnerabilityTimer > 0f)
            return;

        currentHealth -= damage;

        // Sync updated HP to HUD
        if (Game_Manager.Instance != null)
            Game_Manager.Instance.SetPlayerHP(currentHealth, maxHealth);

        if (damageEffect != null)
            damageEffect.Play();

        if (spriteRenderer != null)
            spriteRenderer.color = damageColor;

        invulnerabilityTimer = invulnerabilityTime;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (Game_Manager.Instance != null)
            Game_Manager.Instance.StopScoring();

        SceneManager.LoadScene("End");
    }
}