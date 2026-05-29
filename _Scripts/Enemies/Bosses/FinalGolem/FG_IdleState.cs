using System.Drawing;
using UnityEngine;

public class FG_IdleState : State
{
    protected override string AnimBoolName => "IsIdle";
    private Transform target;
    private FGEnemy fg;
    private Health health;
    private float timer = 0f;
    public FG_IdleState(Enemy enemy) : base(enemy)
    {
        fg = enemy as FGEnemy;
        health = enemy.GetComponent<Health>();
    }

    public override void FixedUpdate()
    {
        Debug.Log("IdleState");

        // phase transition to 
        if (health.health <= (health.maxHealth / 2) && fg.transform.position.y < 25f) 
        { 
            ServiceLocator.Get<SpellBook>().playerStats.FGPhase1Defeated = true;
            stateMachine.ChangeState(new FG_PhaseTransition(fg));
        }


        //1. Check for target
        target = ServiceLocator.Get<PlayerMovement>().GetComponent<Transform>();

        if (!target) { return; }

        timer += Time.deltaTime;
        if (timer < 2f) { return; }

        // slam into platform
        if (combat.FG_CanSlamAttack() && health.health >= (health.maxHealth / 2))
        {
            stateMachine.ChangeState(new FG_SlamAttackState(fg));
            return;
        }

        // push into lava
        if (combat.FG_CanPushAttack() && health.health <= (health.maxHealth / 2))
        {
            stateMachine.ChangeState(new FG_PushAttackState(fg));
            return;
        }

        // fireballs
        if (combat.FG_CanLavaAttack())
        {
            stateMachine.ChangeState(new FG_LavaAttackState(fg));
            return;
        }


    }
}
