using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform attackTarget;
    private EnemyConfig config;
    private Enemy enemy;
    private float lastMeleeAttackTime;
    private float lastRangedAttackTime;
    private float lastDamageDoneTime;
    private float graceTime = 0.25f;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        config = enemy.Config;
    }
    public bool CanMeleeAttack() => Time.time - lastMeleeAttackTime >= config.meleeAttackCooldown;  
    public bool CanRangedAttack() => Time.time - lastRangedAttackTime >= config.rangedAttackCooldown;
    public void PerformMeleeAttack()
    {
        Debug.Log("Performing melee attack");
        lastMeleeAttackTime = Time.time;
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, config.meleeRange, LayerMask.GetMask("Player"));

        if (!hit) { return; }
        Debug.Log($"Melee attack hit: {hit?.name ?? "None"}");

        Health health = hit.GetComponent<Health>();
        if (health != null && ((Time.time - lastDamageDoneTime) > graceTime))
        {
            health.ApplyResistance(config.meleeDamage, config.meleeDamageType, transform.position);
            lastDamageDoneTime = Time.time;
        }
    }

    public void PerformRangedAttack() { }
    public void B_PerformVineAttack(Transform target) 
    {
        VineAttack vine = GetComponent<VineAttack>();
        lastRangedAttackTime = Time.time;

        GameObject go = Instantiate(vine.vinePrefab, new Vector2(target.position.x, vine.ceiling.position.y), Quaternion.identity);

        go.gameObject.transform.position = new Vector2(target.position.x, vine.ceiling.position.y);
        DealDamage[] dmg = go.GetComponentsInChildren<DealDamage>();
        foreach (DealDamage d in dmg) { d.rf_ReceiveAttributes(config.rangedDamage, config.rangedDamageType, LayerMask.GetMask("Player")); }
        
        Destroy(go, 2f);
    }
    public void B_PerformChargeAttack() 
    {
        Debug.Log("Performing charge attack");
        lastMeleeAttackTime = Time.time;
        Vector2 direction = enemy.FacingDirection * Vector2.right;

        enemy.RB.AddForce(direction * config.chargeAttackForce, ForceMode2D.Impulse);
    }
    public void B_PerformJumpAttack() 
    {
        enemy.RB.AddForce(Vector2.up * config.jumpHeight, ForceMode2D.Impulse);
    }

    public void DealDamageOnSelfCollider()
    {
        Collider2D hit = Physics2D.OverlapCircle(gameObject.transform.position, 1f, LayerMask.GetMask("Player"));

        if (!hit) { return; }
        Debug.Log($"Melee attack hit: {hit?.name ?? "None"}");

        Health health = hit.GetComponent<Health>();
        if (health != null && ((Time.time - lastDamageDoneTime) > graceTime))
        {
            Debug.Log($"Applying damage: {config.meleeDamage} of type {config.meleeDamageType}");
            health.ApplyResistance(config.meleeDamage, config.meleeDamageType, transform.position);
            lastDamageDoneTime = Time.time;
        }
    }
}
