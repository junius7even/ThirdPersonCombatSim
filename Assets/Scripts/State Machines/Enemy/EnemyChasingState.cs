using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly float AnimatorDampTime = 0.1f;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, AnimatorDampTime);
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        if (IsInAttackingRange())
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }
        FacePlayer();
        MoveToPlayer(deltaTime);
        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    private void Start ()
    {
        
    }

    private void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = stateMachine.Player.transform.position;

            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }

        stateMachine.Agent.velocity = stateMachine.Controller.velocity; // Character controller's actual velocity is fed to the navmesh agent
    }

    private bool IsInAttackingRange()
    {
        return (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
