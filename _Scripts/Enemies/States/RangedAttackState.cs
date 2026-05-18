using UnityEngine;

public class RangedAttackState : State
{
    protected override string AnimBoolName => "IsShooting";
    private float t = 0f;
    public RangedAttackState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;

        combat.PerformRangedAttack(senses.GetChaseTarget());
    }

    public override void Update()
    {
        t += Time.deltaTime;
        if (t > 2f) { stateMachine.ChangeState(new IdleState(enemy)); }
        }

    public override void OnAnimationFinished()
    {
        stateMachine.ChangeState(new IdleState(enemy));
    }
}
