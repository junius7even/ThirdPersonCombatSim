using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine; // Ensure classes that implement this abstract class have access to the player state machine

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    
    protected bool IsInChaseRange()
    {
        float PlayerDistanceSqr = (stateMachine.Player.transform.position  - stateMachine.transform.position).sqrMagnitude;
        if (PlayerDistanceSqr <= stateMachine.PlayerChasingRange * stateMachine.PlayerChasingRange)
        {
            return true;
        }
        return false;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        stateMachine.Controller.Move(stateMachine.ForceReceiver.Movement * deltaTime);
    }


    // Makes the enemy model face the player
    protected void FacePlayer()
    {
        if (stateMachine.Player == null) { return; }
        // Gets the direction vector by subtracting player position from target. Gets the diff of each axis
        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }
}
