using UnityEngine;

public class B_IntroState : State
{
    private BushyEnemy bushy;
    public B_IntroState(Enemy enemy) : base(enemy) { bushy = enemy as BushyEnemy; }

    private bool FightStart = false;
    private PlayerMovement pMove;

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;
        pMove = ServiceLocator.Get<PlayerMovement>();
    }

    public override void FixedUpdate()
    {
        Debug.Log("IntroState" + FightStart.ToString());
        if (pMove == null) { pMove = ServiceLocator.Get<PlayerMovement>(); return; }
        if (!FightStart)
        {
            float dist = Vector2.Distance(combat.gameObject.transform.position, pMove.gameObject.transform.position);
            if (dist <= 5f) { FightStart = true; }
        }
        else
        {
            
            stateMachine.ChangeState(new B_IdleState(bushy));
        }
    }
}
