using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject _slashEffect;

    [Header("Attacking")]
    [SerializeField] private float _timeBetweenAttack;
    [SerializeField] private float _timeSinceAttack = 0f;
    [SerializeField] private Transform _upAttackTransform, _downAttackTransform, _sideAttackTransform;
    [SerializeField] private Vector2 _upAttackArea, _downAttackArea, _sideAttackArea;
    [SerializeField] private LayerMask _attackableLayer;
    [SerializeField] private bool _attacking = false;
    private float _xAxis, _yAxis;

    [Header("Damage")]
    [SerializeField] private float _damage = 3f;
    [SerializeField] private re_DamageType _damageType = re_DamageType.Physical;
    [SerializeField] private float _damageExtra = 0f;
    [SerializeField] private re_DamageType _damageExtraType = re_DamageType.None;
    [SerializeField] private float _atttackIframe = 0.1f;
    [SerializeField] private float _knockbackForce = 100f;

    [Header("Recoil Variables")]
    [SerializeField] private bool _isRecoiling = false;
    [SerializeField] private int _recoilXSteps = 5;
    [SerializeField] private int _recoilYSteps = 5;
    [SerializeField] private float _recoilXSpeed = 100f;
    [SerializeField] private float _recoilYSpeed = 100f;
    private int _stepsXRecoiled, _stepsYRecoiled;

    private Rigidbody2D _rb;
    private PlayerMovement _pMov;
    private void Start() 
    { 
        _rb = GetComponent<Rigidbody2D>();
        _pMov = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        _attacking = InputManager.attackWasPressed;
        _isRecoiling = GameController.Instance.IsPlayerRecoilingX || GameController.Instance.IsPlayerRecoilingY;
        rf_Attack();
    }

    private void rf_Attack()
    {
        _timeSinceAttack += Time.deltaTime;
        if (_attacking && _timeSinceAttack >= _timeBetweenAttack)
        {
            _timeSinceAttack = 0f;

            // grab value from input manager
            _xAxis = InputManager.Movement.x;
            _yAxis = InputManager.Movement.y;

            // hit sideways
            if (_yAxis == 0 || _yAxis < 0.225 && GameController.Instance.IsPlayerGrounded)
            {
                rf_Hit(_sideAttackTransform, _sideAttackArea, ref GameController.Instance.IsPlayerRecoilingX, _recoilXSpeed);
                Instantiate(_slashEffect, _sideAttackTransform);
            }
            // hit upwards
            else if (_yAxis >= 0.225)
            {
                rf_Hit(_upAttackTransform, _upAttackArea, ref GameController.Instance.IsPlayerRecoilingY, _recoilYSpeed);
                rf_SlashEffectAtAngle(_slashEffect, 90, _upAttackTransform);
            }
            // hit downwards
            else if (_yAxis < 0 && !GameController.Instance.IsPlayerGrounded)
            {
                rf_Hit(_downAttackTransform, _downAttackArea, ref GameController.Instance.IsPlayerRecoilingY, _recoilYSpeed);
                rf_SlashEffectAtAngle(_slashEffect, -90, _downAttackTransform);
            }
        }
    }

    private void rf_Hit(Transform attackTransform, Vector2 attackArea, ref bool recoilDir, float recoilStrength)
    {
        Collider2D[] ObjectsToHit = Physics2D.OverlapBoxAll(attackTransform.position, attackArea, 0, _attackableLayer);

        if (ObjectsToHit.Length > 0) 
        { 
            Debug.Log("Hit Something");
            recoilDir = true;
        }

        for (int i = 0; i < ObjectsToHit.Length; i++)
        {
            if (ObjectsToHit[i].GetComponent<EnemyBasic>() != null)
            {
                ObjectsToHit[i].GetComponent<EnemyBasic>().rf_TakeHit(_damage, _damageType, (transform.position - ObjectsToHit[i].transform.position).normalized, _knockbackForce);
                ObjectsToHit[i].GetComponent<EnemyBasic>().rf_TakeHit(_damageExtra, _damageExtraType);
                ObjectsToHit[i].GetComponent<EnemyBasic>().rf_IFrames(_atttackIframe);
            }
        }
    }

    public void rf_Recoil()
    {
        if (GameController.Instance.IsPlayerRecoilingX)
        {
            // check direction returns true if facing right
            if (_pMov.rf_CheckDirection()) { _rb.linearVelocityX = -_recoilXSpeed; }
            else { _rb.linearVelocityX = _recoilXSpeed; }
        }

        if (GameController.Instance.IsPlayerRecoilingY)
        {
            _rb.gravityScale = 0;
            if (InputManager.Movement.y < 0)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _recoilYSpeed);
            }
            else
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocityX, -_recoilYSpeed);
            }
        }

        // stop recoil
        if (GameController.Instance.IsPlayerRecoilingX && _stepsXRecoiled < _recoilXSteps) { _stepsXRecoiled++; }
        else { rf_StopRecoilX(); }

        if (GameController.Instance.IsPlayerRecoilingY && _stepsYRecoiled < _recoilYSteps) { _stepsYRecoiled++; }
        else { rf_StopRecoilY(); }

        if (_pMov.rf_PlayerGrounded()) { rf_StopRecoilY(); }
    }

    private void rf_StopRecoilX()
    {
        _stepsXRecoiled = 0;
        GameController.Instance.IsPlayerRecoilingX = false;
    }

    private void rf_StopRecoilY()
    {
        _stepsYRecoiled = 0;
        GameController.Instance.IsPlayerRecoilingY = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_sideAttackTransform.position, _sideAttackArea);
        Gizmos.DrawWireCube(_upAttackTransform.position, _upAttackArea);
        Gizmos.DrawWireCube(_downAttackTransform.position, _downAttackArea);
    }

    public void rf_ReceiveBonusDamage(float newExtraDamage, re_DamageType damageType)
    {
        _damageExtra = newExtraDamage;
        _damageExtraType = damageType;
    }

    private void rf_SlashEffectAtAngle(GameObject slashEffect, int effectAngle, Transform attackTransform)
    {
        slashEffect = Instantiate(slashEffect, attackTransform);
        slashEffect.transform.eulerAngles = new Vector3(0, 0, effectAngle);
        slashEffect.transform.localScale = new Vector2(0.25f, 0.25f);
    }
}




public enum re_DamageType
{
    None,
    Physical,
    Magical,
    Fire,
    Electric,
    Ice
}