using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Header("General")]
    public int maxHealth;
    public float turnThreshold = 0.2f;

    [Header("Resistances")]
    public int PhysicalResistance = 0;
    public int MagicalResistance = 0;
    public int FireResistance = 0;
    public int ElectricResistance = 0;
    public int IceResistance = 0;


    [Header("Patrol")]
    public float patrolSpeed = 5;
    public float groundCheckDistance = 0.5f;
    public float wallCheckDistance = 0.3f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    [Header("Chase")]
    public float chaseSpeed = 7f;
    public float chaseRange = 5f;
    public LayerMask targetLayer;   

    [Header("Attack")]
    public float meleeAttackCooldown = 1.5f;
    public float meleeRange = 1.2f;
    public int meleeDamage = 1;
    public re_DamageType meleeDamageType = re_DamageType.None;
    public float rangedAttackCooldown = 1.5f;
    public float rangedRange = 1.2f;
    public int rangedDamage = 1;
    public re_DamageType rangedDamageType = re_DamageType.None;
    public float chargeAttackForce = 8f;

    [Header("Jump")]
    public float jumpHeight = 3f;

    [Header("Damaged")]
    public float knockbackForce = 0.2f;
    public float knockbackDuration = 30f;
}
