using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attack
{
    [field: SerializeField] public string AnimationName { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; } = -1; // Determines which attacks can combo into which others
    [field: SerializeField] public float ComboAttackTime { get; private set; } // How far through the attack will you be able to do the next attack
    [field: SerializeField] public float ForceTime { get; private set; }
    [field: SerializeField] public float Force { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float Knockback { get; private set; }
}
