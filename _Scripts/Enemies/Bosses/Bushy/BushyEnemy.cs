using UnityEngine;

public class BushyEnemy : Enemy
{
    public override void Awake() => base.Awake();
    public override void Start() => stateMachine.Initialize(new B_IntroState(this));

    public override void Update() => base.Update();

    public override void FixedUpdate() => base.FixedUpdate();
    public override void OnAnimationFinished() => base.OnAnimationFinished();

    public override void FaceTarget(Transform target) => base.FaceTarget(target);
    public override void Flip() => base.Flip();
}
