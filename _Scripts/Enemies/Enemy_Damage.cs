using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private bool BushyBoss = false;


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

    private void Start()
    {
        if (EnemySaveSystem.IsEnemyDead(SceneManager.GetActiveScene().name, enemy.enemyID)) { Destroy(gameObject); }
    }

    private void HandleDamage(Vector2 sourcePosition)
    {
        int KnockbackDir = 0;
        KnockbackDir = transform.position.x > sourcePosition.x ? 1 : -1;

        Debug.Log($"Enemy took damage from {sourcePosition}, KnockbackDir: {KnockbackDir}");
        if (BushyBoss) { enemy.stateMachine.ChangeState(new B_DamagedState(enemy, KnockbackDir)); return; }
        enemy.stateMachine.ChangeState(new DamagedState(enemy, KnockbackDir));
    }

    private void HandleDeath(Vector2 sourcePosition)
    {
        EnemySaveSystem.SetEnemyDead(SceneManager.GetActiveScene().name, enemy.enemyID);
        if (BushyBoss) { ServiceLocator.Get<SpellBook>().playerStats.SetFlag("FirstBossDefeated", true); }


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
