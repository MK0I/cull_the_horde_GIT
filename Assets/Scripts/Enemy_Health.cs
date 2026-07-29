using UnityEngine;

public class Enemy_Health : MonoBehaviour, IDamage
{
    [SerializeField] private int baseMaxHealth = 3;

    private int currentHealth;

    private void Awake()
    {
        float multiplier = Game_Manager.Instance != null
            ? Game_Manager.Instance.EnemyHPMultiplier
            : 1f;

        // Decimal Check
        int scaledMax = Mathf.CeilToInt(baseMaxHealth * multiplier);
        currentHealth = scaledMax;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (Game_Manager.Instance != null)
            Game_Manager.Instance.AddEnemyKill();

        Destroy(gameObject);
    }
}
