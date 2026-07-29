using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy_Damage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Player_Health health = collision.gameObject.GetComponent<Player_Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}