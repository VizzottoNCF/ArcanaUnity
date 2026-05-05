using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class B_JumpAttackState : State
{
    protected override string AnimBoolName => "IsWalking";
    private BushyEnemy bushy;
    public B_JumpAttackState(Enemy enemy) : base(enemy) { bushy = enemy as BushyEnemy; }
    private float timer = 0f;
    private float minDuration = .3f;

    public override void Enter()
    {
        base.Enter();
        if (!senses.IsAtCliff())
        {
            combat.B_PerformJumpAttack();
            timer = 0f;
        }
        rb.linearVelocity = new Vector2(config.chaseSpeed, rb.linearVelocity.y);
    }
    public override void Update()
    {
        Debug.Log("JumpState");
        timer += Time.deltaTime;
        if (timer >= minDuration && !senses.IsAtCliff())
        {
            stateMachine.ChangeState(new B_IdleState(bushy));
        }
    }
    public override void FixedUpdate()
    {
        combat.DealDamageOnSelfCollider();
    }
}
