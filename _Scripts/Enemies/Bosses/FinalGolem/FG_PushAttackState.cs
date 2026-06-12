using UnityEngine;

public class FG_PushAttackState : State
{
    protected override string AnimBoolName => "IsPush";
    private FGAttacks push;
    private Collider2D rHand, lHand;
    private FGEnemy fg;
    private float t;
    public FG_PushAttackState(Enemy enemy) : base(enemy) { fg = enemy as FGEnemy; }

    public override void Enter()
    {
        base.Enter();
        push = fg.GetComponent<FGAttacks>();
        rHand = push.RHand.GetComponent<Collider2D>();
        lHand = push.LHand.GetComponent<Collider2D>();

        rHand.enabled = true;
        lHand.enabled = true;
    }

    public override void Exit()
    {
        base.Exit();
        rHand.enabled = false;
        lHand.enabled = false;
    }

    public override void Update()
    {
        combat.FG_PerformPushAttack();

        t += Time.deltaTime;
        if (t >= 2.3f) { stateMachine.ChangeState(new FG_IdleState(fg)); }
    }
    public override void OnAnimationFinished() { stateMachine.ChangeState(new FG_IdleState(fg)); }
}
