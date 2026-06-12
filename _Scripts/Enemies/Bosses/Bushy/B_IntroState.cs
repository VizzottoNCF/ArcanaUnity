using UnityEngine;

public class B_IntroState : State
{
    private BushyEnemy bushy;
    public B_IntroState(Enemy enemy) : base(enemy) { bushy = enemy as BushyEnemy; }

    private bool FightStart = false;
    private PlayerMovement pMove;
    private float timer = 0f;
    private bool fired = false;
    private Collider2D c2D;

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;
        pMove = ServiceLocator.Get<PlayerMovement>();
        c2D = bushy.GetComponent<Collider2D>();
    }

    public override void FixedUpdate()
    {
        Debug.Log("IntroState" + FightStart.ToString());
        if (pMove == null) { pMove = ServiceLocator.Get<PlayerMovement>(); return; }
        if (!FightStart)
        {
            float dist = Vector2.Distance(combat.gameObject.transform.position, pMove.gameObject.transform.position);
            if (dist <= 7f) 
            { 
                timer += Time.fixedDeltaTime;
                
            }
            else { timer = 0f; }


            if (timer >= 2f && !fired) { anim.SetTrigger("Intro"); fired = true; }

            if (timer >= 3f) { FightStart = true; }
        }
        else
        {
            c2D.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            stateMachine.ChangeState(new B_IdleState(bushy));
        }
    }
}
