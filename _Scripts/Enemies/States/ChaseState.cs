using UnityEngine;

public class ChaseState : State
{
    private Transform target;
    protected override string AnimBoolName => "IsRunning";
    public ChaseState(Enemy enemy) : base(enemy) { }

    public override void FixedUpdate()
    {
        //1. Check for target
        target = senses.GetChaseTarget();
        if (!target)
        {
            stateMachine.ChangeState(new PatrolState(enemy));
            return;
        }
        enemy.FaceTarget(target);

        //2. Check if close enough to attack/stop chasing
        if (senses.IsInMeleeRange(target) && combat.CanMeleeAttack())
        {
            stateMachine.ChangeState(new MeleeAttackState(enemy));
            return;
        }

        float distance = Mathf.Abs(target.position.x - enemy.transform.position.x);
        if (distance <= config.turnThreshold)
        {
            stateMachine.ChangeState(new IdleState(enemy));
            return;
        }

        //3. Check for obstacles
        if (senses.IsHittingWall() || senses.IsAtCliff())
        {
            stateMachine.ChangeState(new IdleState(enemy));
            return;
        }

        //4. Move towards target
        rb.linearVelocity = new Vector2(config.chaseSpeed * enemy.FacingDirection, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        rb.linearVelocity = Vector2.zero;
    }
}
