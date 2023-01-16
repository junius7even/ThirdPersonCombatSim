using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    // Takes nothing returns nothing. Every state needs to have an entry point
    public abstract void Enter();
    
    // Called every tick. 
    public abstract void Tick(float deltaTime);

    public abstract void Exit();

    // You can be in multiple states at once in the animator
    // Normalizes the time you need to proceed to the next attack
    protected float GetNormalizedTime(Animator animator)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0); // Only using layer 0
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        // If we're transitioning to an attack we wnat to get the data for the next state
        if (animator.IsInTransition(0) && nextInfo.IsTag("Attack")) 
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
