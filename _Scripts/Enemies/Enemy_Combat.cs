using UnityEngine;
using UnityEngine.XR;

public class Enemy_Combat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform attackTarget;
    [SerializeField] private GameObject projectile;
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
    public void PerformMeleeAttack()
    {
        //Debug.Log("Performing melee attack");
        lastMeleeAttackTime = Time.time;
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, config.meleeRange, LayerMask.GetMask("Player"));

        if (!hit) { return; }
        //Debug.Log($"Melee attack hit: {hit?.name ?? "None"}");

        Health health = hit.GetComponent<Health>();
        if (health != null && ((Time.time - lastDamageDoneTime) > graceTime))
        {
            health.ApplyResistance(config.meleeDamage, config.meleeDamageType, transform.position);
            lastDamageDoneTime = Time.time;
        }
    }

    public void PerformRangedAttack(Transform target) 
    {
        //Debug.LogWarning("ranged attack");
        // direction to target
        Vector2 dir = (target.position - attackPoint.position).normalized;
        float bulletAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // instantiate and attribute parameters
        GameObject bul = Instantiate(projectile, attackPoint.transform.position, Quaternion.Euler(0, 0, bulletAngle));

        bul.GetComponent<DealDamage>().rf_ReceiveAttributes(config.rangedDamage, config.rangedDamageType, config.targetLayer);
        Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();

        // send bullet flying
        rb.AddForce(dir * config.rangedAttackForce, ForceMode2D.Impulse);
        lastRangedAttackTime = Time.time;
    }
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


    public void FG_PerformSlamAttack()//Vector3 lPos, Vector3 rPos)
    {
        //FGAttacks slam = GetComponent<FGAttacks>();

        //// choose 2 different targets at random
        //int index1 = slam.index1;
        //int index2 = slam.index2;
        //float moveSpeed = 5f;
        //Transform rHand = slam.RHand.GetComponent<Transform>();
        //Transform lHand = slam.LHand.GetComponent<Transform>();

        //// set targets
        //Vector3 rTarget = new Vector3(slam.platforms[index1].position.x, rHand.position.y, rHand.position.z);
        //Vector3 lTarget = new Vector3(slam.platforms[index2].position.x, lHand.position.y, lHand.position.z);

        //// move hands to target platform
        //rHand.position = Vector3.MoveTowards(rPos, rTarget, moveSpeed * Time.deltaTime);
        //lHand.position = Vector3.MoveTowards(lPos, lTarget, moveSpeed * Time.deltaTime);

        lastMeleeAttackTime = Time.time;
    }
    public void FG_PerformPushAttack() 
    {
        lastMeleeAttackTime = Time.time;
    }
    public void FG_PerformLavaAttack()
    {
        FGAttacks lava = GetComponent<FGAttacks>();
        float angleStep = (lava.endAngle - lava.startAngle) / (lava.projectileCount - 1);

        // shoot projectiles in a circle around attack source
        foreach (GameObject s in lava.AttackSource)
        {
            // Base rotation of the shooter
            float baseAngle = s.transform.parent.eulerAngles.z;

            float angle = baseAngle + lava.startAngle - 90f;

            Debug.Log(s.name.ToString() + " " + (s.transform.parent.rotation.z * Mathf.Rad2Deg).ToString());
            

            for (int i = 0; i < lava.projectileCount; i++)  
            {
                // Convert angle to direction
                float rad = angle * Mathf.Deg2Rad;

                Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

                
                float bulletAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                GameObject bul = Instantiate(lava.projectilePrefab, s.transform.position, Quaternion.Euler(0, 0, bulletAngle));
                Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();

                rb.AddForce(direction * 15, ForceMode2D.Impulse);

                angle += angleStep;
            }
        }
        lastRangedAttackTime = Time.time;
    }


    public bool FG_CanSlamAttack() => Time.time - lastMeleeAttackTime >= config.slamAttackCooldown;
    public bool FG_CanPushAttack() => Time.time - lastMeleeAttackTime >= config.pushAttackCooldown;
    public bool FG_CanLavaAttack() => Time.time - lastRangedAttackTime >= config.lavaAttackCooldown;

}
