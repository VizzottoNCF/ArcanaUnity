using UnityEngine;

public class PlayerDamaged : MonoBehaviour
{
    public Health health;
    private Rigidbody2D rb;

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.1f;

    private void Awake() => rb = GetComponent<Rigidbody2D>();
    private void OnEnable()
    {
        health.OnDamage += HandleDamage;
        health.OnDeath += HandleDeath;
    }
    private void OnDisable()
    {
        health.OnDamage -= HandleDamage;
        health.OnDeath -= HandleDeath;
    }

    private void HandleDamage(Vector2 sourcePosition)
    {
        int KnockbackDir = 0;
        KnockbackDir = transform.position.x > sourcePosition.x ? 1 : -1;
        
        float knockbackVelocity = KnockbackDir * knockbackForce;

        GameController.Instance.CanPlayerMove = false;
        GameController.Instance.InKnockback = true;
        rb.linearVelocity = new Vector2(knockbackVelocity, rb.linearVelocity.y);
        Invoke(nameof(StopKnockback), knockbackDuration);
    }

    private void StopKnockback()
    {
        GameController.Instance.isTimeSlowed = false;
        Time.timeScale = 1f;

        //Debug.Log("Stopping knockback, allowing player to move again.");
        GameController.Instance.CanPlayerMove = true;
        GameController.Instance.InKnockback = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void HandleDeath(Vector2 sourcePosition)
    {
        GameController.Instance.IsDead = true;
        GameController.Instance.CanPlayerMove = false;
        GameController.Instance.InKnockback = true;
        GameController.Instance.isTimeSlowed = true;
        Time.timeScale = 0.3f;
        GameController.Instance.rf_PlayerDeath();
        int KnockbackDir = 0;
        KnockbackDir = transform.position.x > sourcePosition.x ? 1 : -1;

        float knockbackVelocity = KnockbackDir * knockbackForce;

        rb.linearVelocity = new Vector2(knockbackVelocity, rb.linearVelocity.y);
        Invoke(nameof(StopKnockback), knockbackDuration*5);

    }
}
