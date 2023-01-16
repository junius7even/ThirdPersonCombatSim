using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed"); // Passing in integers is faster than strings

    // Because the constructor was also present, you will need a constructor as well
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private const float AnimatorDamptime = 0.1f;

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget; // Subscribe to the target event when the button is pressed
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, 0.1f);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    private void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) { return; } // Only enter the targeting state if target selection was successful

        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    public override void Tick(float deltaTime)
    {
        // If player is attacking in free look state, then enter the attacking state
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        // Debug.Log(stateMachine.InputReader.MovementValue); // Logs read movement value
        // stateMachine.InputReader.JumpEvent += Enter; // Subscribing to an event is done like this. Adding the enter would call enter whenever you jumped
        // You can also unsubscribe and subscribe more than once a function to an event

        Vector3 movement = CalculateMovement();

        // Deprecated due to CalculateMovement adding in consideration to the camera object.
        // movement.x = stateMachine.InputReader.MovementValue.x;
        // movement.y = 0; // in unity 3d, the y axis is vertical
        // movement.z = stateMachine.InputReader.MovementValue.y; // since this is a vector two, it's technically z axis in unity. 
        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDamptime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDamptime, deltaTime);

        FaceMovementDirection(movement, deltaTime);
    }

    private Vector3 CalculateMovement()
    {
        Vector3 ZAxisMovement = stateMachine.MainCameraTransform.forward;
        ZAxisMovement.y = 0; // Don't care about the camera tilt

        Vector3 XAxisMovement = stateMachine.MainCameraTransform.right;
        XAxisMovement.y = 0;



        return ZAxisMovement * stateMachine.InputReader.MovementValue.y + XAxisMovement * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation =  Quaternion.Lerp(
            stateMachine.transform.rotation, 
            Quaternion.LookRotation(movement), 
            deltaTime * stateMachine.RotationDamping);
    }
}
