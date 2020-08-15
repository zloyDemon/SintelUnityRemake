using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SintelGameParameters
{
    // Animator
    public static readonly int MoveStateId = Animator.StringToHash("MoveState");
    public static readonly int AttackId = Animator.StringToHash("AttackId");
    public static readonly int CharacterState = Animator.StringToHash("CharacterState");

    // Animator states
    public static readonly int SintelIdleAttack_2 = Animator.StringToHash("sintel_idle_attack_2");
    public static readonly int SintelAttack_1 = Animator.StringToHash("sintel_attack_1");
    public static readonly int SintelAttack_2 = Animator.StringToHash("sintel_attack_2");
    public static readonly int SintelAttack_3 = Animator.StringToHash("sintel_attack_3");
    public static readonly int AttackEnd = Animator.StringToHash("attack_end");

    public class AnimatorParameters
    {
        public static readonly int MoveStateId = Animator.StringToHash("MoveState");
        public static readonly int IsAttack = Animator.StringToHash("IsAttack");
        public static readonly int AttackId = Animator.StringToHash("AttackId");
        public static readonly int CharacterState = Animator.StringToHash("CharacterState");
        public static readonly int IsGround = Animator.StringToHash("IsGround");
    }
}
