using UnityEngine;

public class Enemy_Senses : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyConfig config;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform[] wallCheck;
    [SerializeField] private Transform attackPoint;
    
    public bool IsAtCliff() => !Physics2D.Raycast(groundCheck.position, Vector2.down, config.groundCheckDistance, config.groundLayer);
    public bool IsHittingWall() { 
        foreach (Transform t in wallCheck)
        {
            if (Physics2D.Raycast(t.position, Vector2.right, config.wallCheckDistance, config.wallLayer) == true) { return true; }
        }
        return false;
    }
    public Transform GetChaseTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, config.chaseRange, config.targetLayer);
        if (!hit || GameController.Instance.IsDead) { return null; }
        return hit.transform;
    }
    public bool IsInMeleeRange(Transform target)
    {
        if (!target) { return false; }
        float distance = Vector2.Distance(target.position, attackPoint.position);
        return distance <= config.meleeRange;
    }
    public bool IsInRangedRange(Transform target)
    {
        if (!target) { return false; }
        float distance = Vector2.Distance(target.position, attackPoint.position);
        return distance <= config.rangedRange;
    }

    public bool IsTargetGrounded(Transform target)
    {
        if (!target) { return false; }
        return ServiceLocator.Get<PlayerMovement>().rf_PlayerGrounded();
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null || wallCheck == null || attackPoint == null) { return; }

        // Ground Check
        Gizmos.color = Color.purple;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * config.groundCheckDistance);

        // Wall Check
        foreach (Transform t in wallCheck)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(t.position, t.position + Vector3.right * enemy.FacingDirection * config.wallCheckDistance);
        }
        // Chase Check
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, config.chaseRange);

        //Melee Check
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, config.meleeRange);
    }
}
