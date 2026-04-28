using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variables
    public int FacingDirection { get; private set; } = 1;

    //Components | Properties
    public Rigidbody2D RB { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public Enemy_Senses Senses { get; private set; }
    public Enemy_Combat Combat { get; private set; }
    public Animator Anim { get; private set; }
    public EnemyConfig Config;
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        stateMachine = new StateMachine();
        Senses = GetComponent<Enemy_Senses>();
        Combat = GetComponent<Enemy_Combat>();
    }
    private void Start() { stateMachine.Initialize(new PatrolState(this)); }

    private void Update() => stateMachine.CurrentState?.Update();

    private void FixedUpdate() => stateMachine.CurrentState?.FixedUpdate();
    public void OnAnimationFinished() => stateMachine.CurrentState?.OnAnimationFinished();

    public void FaceTarget(Transform target)
    {
        float offset = target.position.x - transform.position.x;
        int dir = offset > 0 ? 1 : -1;
        if (dir != FacingDirection)
        {
            Flip();
        }
    }
    public void Flip()
    {
        FacingDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x = FacingDirection;
        transform.localScale = scale;
    }

}
