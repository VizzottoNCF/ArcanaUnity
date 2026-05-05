using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class B_VineAttackState : State
{
    protected override string AnimBoolName => "IsRunning";
    private BushyEnemy bushy;
    private float duration = 1.5f;
    private float timer = 0f;
    public B_VineAttackState(Enemy enemy) : base(enemy) { bushy = enemy as BushyEnemy; }
    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;
        combat.B_PerformVineAttack(senses.GetChaseTarget());
        timer = 0f;
    }
    public override void Update()
    {
        Debug.Log("VineState");
        timer += Time.deltaTime;
        if (timer >= duration) { stateMachine.ChangeState(new B_IdleState(bushy)); }
    }
}
