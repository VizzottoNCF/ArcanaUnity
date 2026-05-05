using UnityEngine;

public class B_DamagedState : DamagedState
{
    private BushyEnemy bushy;
    public B_DamagedState(Enemy enemy, int knockbackDir) : base(enemy, knockbackDir)
    { 
        bushy = enemy as BushyEnemy;
        knockbackVelocity = knockbackDir * config.knockbackForce;
    }

    protected override string AnimBoolName => "IsDamaged";
    private float knockbackVelocity;
    private float knockbackDuration;

    public override void Enter() => base.Enter();

    public override void FixedUpdate()
    {
        knockbackDuration -= Time.fixedDeltaTime;
        if (knockbackDuration <= 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            if (!senses.IsAtCliff()) { stateMachine.ChangeState(new B_IdleState(enemy)); }
        }
    }
}
