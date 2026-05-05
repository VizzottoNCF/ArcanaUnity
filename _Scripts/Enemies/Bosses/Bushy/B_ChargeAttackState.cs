using UnityEngine;

public class B_ChargeAttackState : State
{
    protected override string AnimBoolName => "IsAttacking";
    private BushyEnemy bushy;
    public B_ChargeAttackState(Enemy enemy) : base(enemy) { bushy = enemy as BushyEnemy; }
    private bool isAttacking = false;

    public override void Enter() { base.Enter(); }

    public override void FixedUpdate()
    {
        Debug.Log("ChargeState");
        if (senses.IsHittingWall() || senses.IsAtCliff())
        {
            stateMachine.ChangeState(new B_IdleState(enemy));
            return;
        }
        else if (!isAttacking)
        {
            combat.B_PerformChargeAttack();
            isAttacking = true;
        }
        else { combat.DealDamageOnSelfCollider(); }
    }
}
