using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackState : AIBaseState
{

    public delegate void DoAttack();
    DoAttack attackFunc;

    Coroutine corCooldown;
    private bool isCooldown;
    private float maxInterval;
    private float minInterval;

    public AttackState SetAttackCallback(DoAttack attack)
    {
        attackFunc = attack;
        return this;
    }

    public AttackState SetCooldownTimeInterval(float min, float max)
    {
        minInterval = min;
        maxInterval = max;
        return this;
    }

    public override void EnterState()
    {
        corCooldown = StartCoroutine(CorCooldown(0.6f));
    }

    public override void ExitState()
    {
        SintelUtils.KillAndNullCoroutine(this, ref corCooldown);
        isCooldown = false;
    }

    public override void UpdateState()
    {
        AIBaseBehaviour.MoveComponent.RotateToTarget(AIBaseBehaviour.Target.transform);
        if (!isCooldown && corCooldown == null && AIBaseBehaviour.CurrentState == this)
        {
            attackFunc();
            float coolDownTime = Random.Range(minInterval, maxInterval);
            corCooldown = StartCoroutine(CorCooldown(coolDownTime));
        }
    }

    IEnumerator CorCooldown(float cooldownSeconds)
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownSeconds);
        isCooldown = false;
        corCooldown = null;
    }
}
