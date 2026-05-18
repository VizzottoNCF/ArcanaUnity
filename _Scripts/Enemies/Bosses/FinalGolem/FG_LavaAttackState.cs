using UnityEngine;

public class FG_LavaAttackState : State
{
    protected override string AnimBoolName => "IsLava";
    private FGEnemy fg;
    private float timer = 0f;
    private bool firedAttack = false;
    public FG_LavaAttackState(Enemy enemy) : base(enemy) { fg = enemy as FGEnemy; }

    public override void Update()
    {
        base.Update();
        Debug.Log("Lava Attack State");
        timer += Time.deltaTime;

        if (timer >= 1f && !firedAttack) { combat.FG_PerformLavaAttack(); firedAttack = true; }
        if (timer >= 3f) { stateMachine.ChangeState(new FG_IdleState(fg)); }
    }
}
