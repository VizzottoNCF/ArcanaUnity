using NUnit.Framework;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float _health = 1;
    [SerializeField] protected bool _dead = false;
    [SerializeField] protected int[] _resistances;
    [SerializeField] protected float _iFrames = 0f;
    [SerializeField] protected float _recoilLenght;
    [SerializeField] protected float _recoilFactor;
    [SerializeField] protected bool _isRecoiling = false;

    [SerializeField] protected GameController _player;
    [SerializeField] protected float _speed;

    protected float _recoilTimer = 0f;
    protected Rigidbody2D _rb;

    public virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameController.Instance;
    }

    public virtual void Update()
    {
        if (_iFrames > 0) { _iFrames -= Time.deltaTime; }
        if (_isRecoiling)
        {
            if (_recoilTimer < _recoilLenght)
            {
                _recoilTimer += Time.deltaTime;
            }
            else
            {
                _isRecoiling = false;
                _recoilTimer = 0f;
            }
        }
        if (!_dead && _health <= 0) { rf_Die(); }
    }

    public virtual void rf_TakeHit(float damageDone, re_DamageType damageType, Vector2 hitDirection = default, float hitForce = 0)
    {
        if (_dead) { return; }
        // TODO: factor in damage resistance later
        float damage = damageDone; //- _resistances[damageType];
        _health -= damage;

        if (!_isRecoiling)
        {
            _rb.AddForce(-hitForce * _recoilFactor * hitDirection);
            _isRecoiling = true;
        }
    }

    public virtual void rf_Die()
    {
        _dead = true;
        Debug.Log($"{gameObject.name} morreu");
    }

    public virtual void rf_IFrames(float newFrames)
    {
        _iFrames = newFrames;
    }
}
