using UnityEngine;

public class ObjectDamaged : MonoBehaviour
{
    public Health health;
    public string flag;
    private PlayerResourceStats stats;

    private void Start() => stats = ServiceLocator.Get<SpellBook>().playerStats;
    
    private void OnEnable() { health.OnDeath += HandleDeath; }
    private void OnDisable() { health.OnDeath -= HandleDeath; }
    
    private void HandleDeath(Vector2 sourcePosition) { stats.SetFlag(flag, true); }
}
