using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy_AI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float baseMoveSpeed = 2.5f;

    [Header("References")]
    [SerializeField] private Transform target;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        float multiplier = Game_Manager.Instance != null
            ? Game_Manager.Instance.EnemySpeedMultiplier
            : 1f;

        Vector2 direction = ((Vector2)target.position - rb.position).normalized;
        rb.linearVelocity = direction * (baseMoveSpeed * multiplier);
    }
}
