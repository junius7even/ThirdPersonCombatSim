using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;
    private List<Target> targets = new List<Target>();

    private Camera mainCamera;

    public Target CurrentTarget { get; private set; }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Unit calls this when the collider is triggered. You can check whatever the hell collides with it.
    // The method signature has to be like this though
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        targets.Add(target);
        target.OnDestroyed += RemoveTarget; // Basically, invokes the removetarget method when OnDestroyed is ever invoked
    }

    private void OnTriggerExit(Collider other)
    {
        // Statement both outputs a boolean for successful getting but also outputs the gotten object to a variable
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        RemoveTarget(target);

        // Target target = other.GetComponent<Target>();
        // if (other.GetComponent<Target>() != null)
        //     targets.Remove(target);
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) { return false; }

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach(Target target in targets)
        {
            // Figures out where on the screen an object is using its world position
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position); // 1 < value < 0 then it's not on your screen
            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.x > 1)
                continue;
            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f); // Returns vector from character to center of screen
            
            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }
        if (closestTarget == null) { return false; }
        CurrentTarget = closestTarget;
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) { return; }
        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    // Removes target from both Cinemachine target group(targets those objects) as well as the local list storing targets
    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }
        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}
