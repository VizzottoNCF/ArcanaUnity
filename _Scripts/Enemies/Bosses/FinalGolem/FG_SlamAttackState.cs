using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class FG_SlamAttackState : State
{
    protected override string AnimBoolName => "IsSlam";
    private FGEnemy fg;
    private float t = 0f;
    private FGAttacks slam;
    private Collider2D rHand, lHand;
    private Vector3 lPos, rPos;
    public FG_SlamAttackState(Enemy enemy) : base(enemy) { fg = enemy as FGEnemy; }

    public override void Enter()
    {
        base.Enter();
        slam = fg.GetComponent<FGAttacks>();
        rHand = slam.RHand.GetComponent<Collider2D>();
        lHand = slam.LHand.GetComponent<Collider2D>();

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
        combat.FG_PerformSlamAttack();

        t += Time.deltaTime;

        if (t >= 2.3f) { stateMachine.ChangeState(new FG_IdleState(fg)); }
        //if (Input.GetKeyDown(KeyCode.G)) { stateMachine.ChangeState(new FG_IdleState(fg));  }
    }
    public override void OnAnimationFinished() { stateMachine.ChangeState(new FG_IdleState(fg)); }
}
