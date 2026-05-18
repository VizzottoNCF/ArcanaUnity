using System.Drawing;
using UnityEngine;

public class FG_IntroState : State
{
    protected override string AnimBoolName => "IsIntro";
    private Health h;
    private FGEnemy fg;
    private PlayerMovement pMove;
    private FGAttacks props;
    private Collider2D col;
    private Collider2D col2;
    private float t = 0f;
    private float t2 = 0f;
    private bool relocated = false;
    public FG_IntroState(Enemy enemy, bool phase2) : base(enemy)
    {
        fg = enemy as FGEnemy;
        pMove = ServiceLocator.Get<PlayerMovement>();
        relocated = phase2;
    }
    public override void Enter()
    {
        h = fg.GetComponent<Health>();
        props = fg.GetComponent<FGAttacks>();
        col = props.Area1.GetComponent<Collider2D>();
        col2 = props.Area2.GetComponent<Collider2D>();
        pMove = ServiceLocator.Get<PlayerMovement>();

        // fight restart
        if (ServiceLocator.Get<SpellBook>().playerStats.GetFlag("FGPhase1Defeated") && !relocated)
        {
            props.lavaTiles.SetActive(true);
            props.lavaTilesVisual.SetActive(true);

            h.health = h.maxHealth / 2;
            stateMachine.ChangeState(new FG_PhaseTransition(fg));
        }
    }
    public override void Update()
    {
        if (pMove == null) { pMove = ServiceLocator.Get<PlayerMovement>(); return; }

        // if haven't played anim yet
        if (!fg.Anim.GetBool("IsIntro"))
        {
            if (t >= 3f) { fg.Anim.SetBool("IsIntro", true); }

            if (col.bounds.Contains(pMove.transform.position) || col2.bounds.Contains(pMove.transform.position)) { t += Time.deltaTime; }
            else { t = 0f; }
        }
        else
        {
            t2 += Time.deltaTime;

            if (t2 >= 3f) { stateMachine.ChangeState(new FG_IdleState(fg)); }
        }
    }

    public override void OnAnimationFinished()
    {
        base.OnAnimationFinished();
        stateMachine.ChangeState(new FG_IdleState(fg));
    }
    public override void Exit() { base.Exit(); }
}
