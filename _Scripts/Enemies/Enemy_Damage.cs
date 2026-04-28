using UnityEngine;

public class Enemy_Damage : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    public Health health;

    [Header("Death Settings")]
    public bool partsEffects = false;
    [SerializeField] private GameObject[] deathParts;
    [SerializeField] private float spawnForce = 5f;
    [SerializeField] private float torque = 5f;
    [SerializeField] private float lifetime = 2f;


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

        Debug.Log($"Enemy took damage from {sourcePosition}, KnockbackDir: {KnockbackDir}");
        enemy.stateMachine.ChangeState(new DamagedState(enemy, KnockbackDir));
    }

    private void HandleDeath(Vector2 sourcePosition)
    {
        if (partsEffects)
        {
            foreach (GameObject part in deathParts)
            {
                Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0.5f, 1f)).normalized;
                GameObject p = Instantiate(part, transform.position, rotation);

                Rigidbody2D rb = p.GetComponent<Rigidbody2D>();

                Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                rb.linearVelocity = randomDirection * spawnForce;
                rb.AddTorque(Random.Range(-torque, torque), ForceMode2D.Impulse);

                Destroy(p, lifetime);
            }
        }

        Destroy(gameObject);
    }
}
