using UnityEngine;

public class Zombie : EnemyBasic
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (!_isRecoiling && !_dead)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(_player.transform.position.x, transform.position.y), _speed * Time.deltaTime);
        }
    }

    public override void rf_TakeHit(float damageDone, re_DamageType damageType, Vector2 hitDirection = default, float hitForce = 0)
    {
        base.rf_TakeHit(damageDone, damageType, hitDirection, hitForce);
    }

    public override void rf_Die()
    {
        base.rf_Die();
    }

    public override void rf_IFrames(float newFrames)
    {
        base.rf_IFrames(newFrames);
    }
}
