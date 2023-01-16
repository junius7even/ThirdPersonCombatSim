using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly float AnimatorDampTime = 0.1f;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, AnimatorDampTime);
        stateMachine.Weapon.SetAttack(stateMachine.AttackDamage, stateMachine.AttackKnocknback);
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator) >= 1)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }
    }
}
