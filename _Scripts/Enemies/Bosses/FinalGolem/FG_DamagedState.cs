using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.STP;

public class FG_DamagedState : DamagedState
{
    private FGEnemy fg;
    public FG_DamagedState(Enemy enemy, int knockbackDir) : base(enemy, knockbackDir)
    {
        fg = enemy as FGEnemy;
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

            if (!senses.IsAtCliff()) { stateMachine.ChangeState(new FG_IdleState(fg)); }
        }
    }
}
