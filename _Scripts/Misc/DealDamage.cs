using UnityEngine;

public class DealDamage : MonoBehaviour
{
    
    private re_DamageType _damageType = re_DamageType.None;
    private int _damage = 0;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private LayerMask _targetLayer;

    private bool _hit = false;


    public void rf_ReceiveAttributes(int damage, re_DamageType damageType, LayerMask targetLayer)
    {
        _damage = damage;
        _damageType = damageType;
        _targetLayer = targetLayer;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetLayer) != 0)
        {
            // Apply damage to the target
            Debug.Log($"Dealt {_damage} of type {_damageType} to {other.name}");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_hit)
        {

        }
    }
}