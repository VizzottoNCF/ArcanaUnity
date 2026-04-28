using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    private EnemyConfig config;
    private Enemy enemy;
    private float lastAttackTime;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        config = enemy.Config;
    }
    public bool CanMeleeAttack() => Time.time - lastAttackTime >= config.meleeAttackCooldown;
    public void PerformMeleeAttack()
    {
        Debug.Log("Performing melee attack");
        lastAttackTime = Time.time;
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, config.meleeRange, LayerMask.GetMask("Player"));

        Debug.Log($"Melee attack hit: {hit?.name ?? "None"}");
        if (!hit) { return; }

        Health health = hit.GetComponent<Health>();
        Debug.Log($"Health component found: {health != null}");
        if (health != null)
        {
            Debug.Log($"Applying damage: {config.meleeDamage} of type {config.meleeDamageType}");
            health.ApplyResistance(config.meleeDamage, config.meleeDamageType, transform.position);
        }
    }
}
