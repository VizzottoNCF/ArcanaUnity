using UnityEngine;

[CreateAssetMenu(menuName = "Attack Spell")]
public class AttackSpell : Spell
{
    [Header("Attack Spell Config")]
    [SerializeField] private int _damage;
    [SerializeField] private re_DamageType _damageType;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _shootForce = 2f;
    private Transform _firePoint;

    public override void rf_Activate(GameObject Parent)
    {
        // gets starting point for spell
        _firePoint = GameObject.Find("FirePoint").transform;

        // spawn attack gameobject and give attributes
        GameObject bullet = Instantiate(_projectile, _firePoint.position, _firePoint.rotation);
        bullet.GetComponent<DealDamage>().rf_ReceiveAttributes(_damage, _damageType, _targetLayer);

        // send out bullet towards direction
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(_firePoint.right * _shootForce, ForceMode2D.Impulse);

    }
}
