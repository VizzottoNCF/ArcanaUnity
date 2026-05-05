using UnityEngine;

public abstract class State
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected EnemyConfig config;
    protected Enemy_Senses senses;
    protected Enemy_Combat combat;
    protected Enemy enemy;
    protected StateMachine stateMachine;

    protected virtual string AnimBoolName => null;
    protected State(Enemy enemy)
    {
        rb = enemy.GetComponent<Rigidbody2D>();
        config = enemy.Config;
        senses = enemy.Senses;
        this.enemy = enemy;
        anim = enemy.Anim;
        stateMachine = enemy.stateMachine;
        combat = enemy.Combat;
    }

    public virtual void Enter() 
    {
        if (!string.IsNullOrEmpty(AnimBoolName)) { anim.SetBool(AnimBoolName, true); }
    }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void OnAnimationFinished() { }
    public virtual void Exit() 
    {
        if (!string.IsNullOrEmpty(AnimBoolName)) { anim.SetBool(AnimBoolName, false); }
    }
}
