using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly float AnimatorDampTime = 0.1f;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, 0.1f);
        
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if (IsInChaseRange())
        {
            // Transition to chasing state
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }
         stateMachine.Animator.SetFloat(SpeedHash, 0, AnimatorDampTime, deltaTime);
    }
}
