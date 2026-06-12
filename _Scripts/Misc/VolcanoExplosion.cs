using UnityEngine;

public class VolcanoExplosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    public GameObject prefab;
    public int quantity = 10;
    public float torque = 5f;
    public float spawnForce = 5f;
    public float lifetime = 3f;
    public Transform spawnPoint;

    [Header("Other")]
    public Sprite volcanoLavaSprite;
    private Sprite og;
    private SpriteRenderer sr;
    private PlayerResourceStats flags;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        og = sr.sprite;
        flags = ServiceLocator.Get<SpellBook>().playerStats;
    }
    private void Update()
    {
        if (flags == null) return;
        if (!flags.VolcanoExploded) return;

        sr.sprite = volcanoLavaSprite;
    }
    public void Explode()
    {
        if (flags == null) { flags = ServiceLocator.Get<SpellBook>().playerStats; }

        ShootFire();
        Invoke("ShootFire", 0.5f);
        Invoke("ShootFire", 1.0f);
        Invoke("ShootFire", 1.5f);

        sr.sprite = volcanoLavaSprite;
        flags.VolcanoExploded = true;
    }

    private void ShootFire()
    {
        for (int i = 0; i < quantity; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0.5f, 1f)).normalized;
            GameObject p = Instantiate(prefab, spawnPoint.position, rotation);

            Rigidbody2D rb = p.GetComponent<Rigidbody2D>();

            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
            rb.linearVelocity = randomDirection * spawnForce;
            rb.AddTorque(Random.Range(-torque, torque), ForceMode2D.Impulse);

            Destroy(p, lifetime);
        }
    }
}
