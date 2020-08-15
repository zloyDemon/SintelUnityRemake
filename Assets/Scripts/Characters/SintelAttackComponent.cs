using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SintelAttackComponent : MonoBehaviour
{
    private enum AttackId
    {
        None,
        Attack_1,
        Attack_2,
        Attack_3,
    }

    private AnimatorStateInfo currentAttackStateInfo;
    private bool canClick;
    private bool clicked;
    private bool canClickForCurrentAnimation;
    private int currentAttackId;
    private string currentAttackAnimationName;
    private SintelAnimatorController sintelAnimatorController;
    private MoveComponent moveComponent;
    private string[] attackNames = { "sintel_attack_1", "sintel_attack_2", "sintel_attack_3" };

    public event Action<int> AttackEvent = id => { };

    public bool IsAttacking { get; private set; }

    private void Awake()
    {
        sintelAnimatorController = GetComponent<SintelAnimatorController>();
        moveComponent = GetComponent<MoveComponent>();
        currentAttackId = 0;
        sintelAnimatorController.AnimationEvent += AttackAnimationEvent;

        foreach (var attackName in attackNames)
        {
            var state = StateManager.Instance.GetStateById(Animator.StringToHash(attackName));
            state.OnStateEnterEvent += OnAttackStateEnter;
        }
    }

    private void OnDestroy()
    {
        foreach (var attackName in attackNames)
        {
            var state = StateManager.Instance.GetStateById(Animator.StringToHash(attackName));
            state.OnStateEnterEvent -= OnAttackStateEnter;
        }
    }

    private void OnAttackStateEnter(Animator animator, AnimatorStateInfo animatorStateinfo, int layer, string name)
    {
        currentAttackAnimationName = name;
        canClickForCurrentAnimation = true;
        moveComponent.AddForce(transform.forward * 3, ForceMode.VelocityChange);
    }

    void Start()
    {
        var s = sintelAnimatorController.MainAnimtor.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
        currentAttackAnimationName = attackNames[0];
    }

    void Update()
    {
        if (IsCurrentAttackName() && sintelAnimatorController.MainAnimtor.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && canClickForCurrentAnimation)
            ResetAnimatorParams();
    }

    public void Attack()
    {
        if (IsAttacking)
        {
            CheckForCombo();
        }
        else
        {
           IsAttacking = true;
           sintelAnimatorController.SetValue(SintelGameParameters.AnimatorParameters.IsAttack, true);
           AcceptNextAnimationData();
        }  
    }

    private void CheckForCombo()
    {
        if (IsCurrentAttackName())
        {
            if (sintelAnimatorController.MainAnimtor.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && canClickForCurrentAnimation)
            {
                AcceptNextAnimationData();
                canClickForCurrentAnimation = false;
            }
        }
    }

    private bool IsCurrentAttackName()
    {
        return sintelAnimatorController.MainAnimtor.GetCurrentAnimatorStateInfo(0).IsName(currentAttackAnimationName);
    }

    private void AcceptNextAnimationData()
    {
        currentAttackId++;
        if (currentAttackId > 3)
            currentAttackId = 1;
        sintelAnimatorController.SetValue(SintelGameParameters.AnimatorParameters.AttackId, currentAttackId);
    }

    private void ResetAnimatorParams()
    {
        currentAttackAnimationName = string.Empty;
        currentAttackId = 0;
        sintelAnimatorController.SetValue(SintelGameParameters.AnimatorParameters.IsAttack, false);
        sintelAnimatorController.SetValue(SintelGameParameters.AnimatorParameters.AttackId, 0);
        IsAttacking = false;
    }


    private void AttackAnimationEvent(int value)
    {
        if (value == (int)SintelAnimatorController.AnimationEventId.Attack)
            AttackEvent(currentAttackId);
    }
}
