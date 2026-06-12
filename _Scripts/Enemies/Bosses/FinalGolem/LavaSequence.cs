using UnityEngine;

public class LavaSequence : MonoBehaviour
{
    private DealDamage damageDealer;
    public bool isActive = false;
    public Transform player;
    public Transform startPosition;
    public Transform endPosition;

    public float minSpeed = 5f;
    public float acceleration = 0.1f;
    public float maxDistance = 4f;

    public float currentSpeed;
    public float t = 0f; // Lerp parameter (0 = start, 1 = end)

    private void Start()
    {
        damageDealer = GetComponent<DealDamage>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (!isActive) { return; }

        // Calculate the distance between lava and player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Adjust speed based on distance (accelerate if too far)
        if (distanceToPlayer > maxDistance)
        {
            currentSpeed += acceleration * Time.deltaTime;
            // Add extra acceleration based on how far beyond max distance it is
            float extraDistance = distanceToPlayer - maxDistance;
            currentSpeed += extraDistance * acceleration * Time.deltaTime;
        }
        else
        {
            // Gradually reduce speed when close to player
            if (currentSpeed > minSpeed) { currentSpeed -= acceleration * Time.deltaTime * 0.5f; }
            else { currentSpeed = minSpeed; }
        }

        // Clamp speed between minimum and maximum
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed * 0.5f, minSpeed * 3f);

        // Move the lava along the path from start to end based on player progression
        // Calculate how far along the path the lava should be based on player's distance to end
        float playerProgress = GetPlayerProgress();

        // Lava chases player but can lag behind if player is fast
        float targetProgress = Mathf.Clamp01(playerProgress); // Lava is slightly behind player

        // Move lava towards target progress
        t = Mathf.MoveTowards(t, targetProgress, currentSpeed * Time.deltaTime / GetPathLength());

        // Update lava position
        Vector3 newPosition = Vector3.Lerp(startPosition.position, endPosition.position, t);
        transform.position = newPosition;

        

        // Check if lava has reached the end
        if (t >= 0.999f)
        {
            SequenceComplete();
        }
    }

    private float GetPlayerProgress()
    {
        // Calculate player's progress along the escape path (0 = at start, 1 = at end)
        Vector3 startToPlayer = player.position - startPosition.position;
        Vector3 startToEnd = endPosition.position - startPosition.position;
        float progress = Vector3.Dot(startToPlayer, startToEnd) / startToEnd.sqrMagnitude;
        return Mathf.Clamp01(progress);
    }

    private float GetPathLength()
    {
        return Vector3.Distance(startPosition.position, endPosition.position);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isActive = false;
        Debug.Log("Player got caught in lava!");
    }

    private void SequenceComplete()
    {
        isActive = false;
        damageDealer.noLoad = true;
        Debug.Log("Lava sequence completed successfully!");
        // Add your completion logic here
        // For example: Load next level or trigger boss fight
    }

    // Optional: Call this to start the sequence
    public void StartSequence()
    {
        isActive = true;
        damageDealer.noLoad = false;
        currentSpeed = minSpeed;
        t = 0f;
        transform.position = startPosition.position;
    }

    // Optional: Reset the sequence
    public void ResetSequence()
    {
        isActive = false;
        damageDealer.noLoad = true;
        currentSpeed = minSpeed;
        t = 0f;
        transform.position = startPosition.position;
    }
}