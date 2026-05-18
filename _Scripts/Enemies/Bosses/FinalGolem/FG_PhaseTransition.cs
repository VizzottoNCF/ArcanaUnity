using UnityEngine;

public class FG_PhaseTransition : State
{
    protected override string AnimBoolName => "IsOutro";
    private FGEnemy fg;
    private Health h;
    private FGAttacks props;
    private bool t = false;
    private Collider2D col;
    private PlayerMovement pMove;
    public FG_PhaseTransition(Enemy enemy) : base(enemy) { fg = enemy as FGEnemy; }

    public override void Enter()
    {
        base.Enter();
        h = fg.GetComponent<Health>();
        props = fg.GetComponent<FGAttacks>();
        col = props.Area2.GetComponent<Collider2D>();
        pMove = ServiceLocator.Get<PlayerMovement>();
        ServiceLocator.Get<SpellBook>().playerStats.FGPhase1Defeated = true;
        props.lavaTiles.SetActive(true);
        props.lavaTilesVisual.SetActive(true);
    }

    public override void Update()
    {
        if (pMove == null) { ServiceLocator.Get<PlayerMovement>(); return; }

        if (col.bounds.Contains(pMove.transform.position)) { t = true; }

        if (t)
        {
            fg.transform.position = new Vector3(fg.transform.position.x, 35.44f, fg.transform.position.z);
            stateMachine.ChangeState(new FG_IntroState(fg, true));
        }
    }
}
