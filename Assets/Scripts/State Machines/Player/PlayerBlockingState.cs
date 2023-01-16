using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private readonly int BlockHash = Animator.StringToHash("Block");
    private readonly float CrossFadeDuration = 0.1f;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);
        stateMachine.Health.SetInvulnerable(true);
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if (!stateMachine.InputReader.isBlocking)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            return;
        }
        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
