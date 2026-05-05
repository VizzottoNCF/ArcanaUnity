using NUnit.Framework;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    
    private re_DamageType _damageType = re_DamageType.None;
    private int _damage = 0;
    private LayerMask _targetLayer;
    private Transform _source;
    private float _graceTime = 0f;
    private float _timeSinceLastHit;

    private void Start() { _source = transform; _timeSinceLastHit = Time.time; }
    public void rf_ReceiveAttributes(int damage, re_DamageType damageType, LayerMask targetLayer)
    {
        _damage = damage;
        _damageType = damageType;
        _targetLayer = targetLayer;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _targetLayer) != 0 && ((Time.time - _timeSinceLastHit) > _graceTime))
        {
            // Apply damage to the target
            Debug.Log($"Dealt {_damage} of type {_damageType} to {collision.name}");

            Health health = collision.GetComponent<Health>();
            health?.ApplyResistance(_damage, _damageType, _source.position);
            _timeSinceLastHit = Time.time;
            _graceTime = 0.5f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & _targetLayer) != 0 && ((Time.time - _timeSinceLastHit) > _graceTime))
        {
            // Apply damage to the target
            Debug.Log($"Dealt {_damage} of type {_damageType} to {collision.gameObject.name}");

            Health health = collision.gameObject.GetComponent<Health>();
            health?.ApplyResistance(_damage, _damageType, _source.position);
            _timeSinceLastHit = Time.time; // Reset grace time on hit
            _graceTime = 0.5f;
        }
    }

    private void killDamage() => Destroy(gameObject);
}