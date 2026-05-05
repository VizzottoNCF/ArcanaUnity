using System.Drawing;
using UnityEngine;

public class B_IdleState : State
{
    protected override string AnimBoolName => "IsIdling";
    private Transform target;
    private BushyEnemy bushy;
    public B_IdleState(Enemy enemy) : base(enemy) { bushy = enemy as BushyEnemy; }
    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;
    }

    public override void FixedUpdate()
    {
        Debug.Log("IdleState");
        //1. Check for target
        target = senses.GetChaseTarget();

        if (!target)
        {
            return;
        }
        bushy.FaceTarget(target);
        rb.linearVelocity = new Vector2(config.chaseSpeed * enemy.FacingDirection, rb.linearVelocity.y);

        //2. Check distance and attack
        if (senses.IsInMeleeRange(target) && combat.CanMeleeAttack() && senses.IsTargetGrounded(target))
        {
            stateMachine.ChangeState(new B_ChargeAttackState(bushy));
            return;
        }

        if (senses.IsInMeleeRange(target) && combat.CanMeleeAttack() && !senses.IsTargetGrounded(target))
        {
            stateMachine.ChangeState(new B_JumpAttackState(bushy));
            return;
        }

        if (!senses.IsInMeleeRange(target) && combat.CanRangedAttack())
        {
            stateMachine.ChangeState(new B_VineAttackState(bushy));
            return;
        }

        
        float distance = Mathf.Abs(target.position.x - enemy.transform.position.x);
        if (distance <= config.turnThreshold)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        //3. Check for obstacles
        if (senses.IsHittingWall() || senses.IsAtCliff() || senses.IsInMeleeRange(target))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
    }
}
