using UnityEngine;

public class DealDamage : MonoBehaviour
{
    
    private re_DamageType _damageType = re_DamageType.None;
    private int _damage = 0;
    private LayerMask _targetLayer;
    private Transform _source;

    private void Start() { _source = transform; }
    public void rf_ReceiveAttributes(int damage, re_DamageType damageType, LayerMask targetLayer)
    {
        _damage = damage;
        _damageType = damageType;
        _targetLayer = targetLayer;

        //Debug.Log($"Received attributes: Damage={_damage}, DamageType={_damageType}, TargetLayer={_targetLayer.value}");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _targetLayer) != 0)
        {
            // Apply damage to the target
            Debug.Log($"Dealt {_damage} of type {_damageType} to {collision.name}");

            Health health = collision.GetComponent<Health>();
            health?.ApplyResistance(_damage, _damageType, _source.position);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & _targetLayer) != 0)
        {
            // Apply damage to the target
            Debug.Log($"Dealt {_damage} of type {_damageType} to {collision.gameObject.name}");

            Health health = collision.gameObject.GetComponent<Health>();
            health?.ApplyResistance(_damage, _damageType, _source.position);
        }
    }
}