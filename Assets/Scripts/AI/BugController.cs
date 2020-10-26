using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SintelAnimatorController;

public class BugController : AIBaseBehaviour
{
    private const float BugSpeed = 3f;
    private const float BugRotationSpeed = 200f;

    private SintelAnimatorController sintelAnimator;
    private bool isFollowToTarget;
    private bool isNearToPlayer;
    private CharacterData characterData;

    public override void Awake()
    {
        base.Awake();
        sintelAnimator = GetComponent<SintelAnimatorController>();
        characterData = GetComponent<CharacterData>();
        Target = SintelPlayer.transform;
        MoveComponent.SetMoveSpeed(4);
        MoveComponent.SetRotationSpeed(200);
        UiDistanceToPlayer.Init(transform);
        UiDistanceToPlayer.SetDistanceToCheck(20);
        UiDistanceToPlayer.PlayerNear += OnPlayerNear;
        sintelAnimator.AnimationEvent += OnAnimationEventHandle;
        characterData.OnHealthChange += OnHealthChange;
    }

    private void Start()
    {
        InitStates();
    }

    private void OnDestroy()
    {
        sintelAnimator.AnimationEvent -= OnAnimationEventHandle;
        characterData.OnHealthChange -= OnHealthChange;
        UiDistanceToPlayer.PlayerNear -= OnPlayerNear;
    }

    private void InitStates()
    {
        var followState = GetState<FollowToTargetState>();
        var attackState = GetState<AttackState>();
        var moveOnPlaceState = GetState<MoveOnPlaceState>();

        var patrolState = GetState<PatrolState>();
        patrolState.SetPatrolPoints(SintelGameManager.Instance.LevelData.GetPatrolPoints());
        patrolState.SetAgentSpeed(4);
        
        followState.SetTargetToMove(SintelPlayer.transform);
        followState.AddTransition(attackState, FromFollowToAttack());
        followState.AddTransition(moveOnPlaceState, FromFollowToMoveOnPlace());

        attackState.SetAttackCallback(StartAttack).SetCooldownTimeInterval(1, 3);
        attackState.AddTransition(followState, FromAttackToFollow());

        moveOnPlaceState.SetMovingCallback(MoveToDirection);
        moveOnPlaceState.AddTransition(followState, FromMoveOnPlaceToFollow());

        Func<bool> FromFollowToAttack() => () => DistanceToTarget(SintelPlayer.transform) < 2;
        Func<bool> FromAttackToFollow() => () => DistanceToTarget(SintelPlayer.transform) > 2;
        Func<bool> FromMoveOnPlaceToFollow() => () => CheckTarget(SintelPlayer.transform);
        Func<bool> FromFollowToMoveOnPlace() => () => DistanceToTarget(SintelPlayer.transform) > 8 && !CheckTarget(SintelPlayer.transform);

        ChangeState(moveOnPlaceState);
    }

    private bool CheckTarget(Transform target)
    {
        bool result = false;
        var directionToTarget = (target.position - EyesForDetect.position).normalized;

        if (Physics.Raycast(EyesForDetect.position, directionToTarget, out RaycastHit hitInfo, 10))
        {
            if (hitInfo.collider != null && hitInfo.collider.CompareTag(target.tag))
            {
                result = true;
                Debug.DrawLine(EyesForDetect.position, target.position, Color.yellow);
            }
        }

        isFollowToTarget = result;
        return result;
    }

    private void MoveToDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            MoveComponent.MoveWithRotation(direction);
            sintelAnimator.SetValue(SintelGameParameters.AnimatorParameters.MoveStateId, 1);
        }
        else
        {
            sintelAnimator.SetValue(SintelGameParameters.AnimatorParameters.MoveStateId, 0);
        }
    }

    private float DistanceToTarget(Transform target)
    {
        var distance = (target.position - transform.position).sqrMagnitude;
        return distance;
    }

    private void OnPlayerNear(bool isNear)
    {
        if (isNear && characterData.IsAlive)
            SintelGameManager.Instance.GameUI.GameObjecUIController.RequestView<NpcHpBar>(UiDistanceToPlayer.transform,
                b => b.SetCharacterData(characterData));
        else
            SintelGameManager.Instance.GameUI.GameObjecUIController.DisableView(UiDistanceToPlayer.transform);
    }

    private void OnHealthChange(int oldValue, int newValue)
    {
        if (newValue == 0)
        {
            Death();
            return;
        }

        sintelAnimator.SetTrigger(SintelGameParameters.BugAnimatorsParameters.Hurt, true);
    }

    public void Attack(Transform attackTo)
    {
        var data = attackTo.GetComponent<CharacterData>();
        if(data != null)
        {
            if (Physics.Raycast(EyesForDetect.position, transform.forward, out RaycastHit hitInfo, 0.5f))
            {
                if (hitInfo.collider != null && hitInfo.collider.CompareTag(attackTo.tag))
                    data.Damage(4);
            }
        }
    }

    public override void Death()
    {
        base.Death();
        sintelAnimator.SetTrigger(SintelGameParameters.BugAnimatorsParameters.Death, true);
        SintelGameManager.Instance.GameUI.GameObjecUIController.DisableView(UiDistanceToPlayer.transform);
        StartCoroutine(CorDeath());
    }

    private void StartAttack()
    {
        sintelAnimator.SetTrigger(SintelGameParameters.AnimatorParameters.AttackBug, true);
    }

    private void Update()
    {
        CurrentState?.Tick();
        //SetVeleoperValues(); // Comment if dont need it
    }

    private void SetVeleoperValues()
    {
        DevelopViewManager.Instance.SetValue("State", CurrentState?.ToString());
        DevelopViewManager.Instance.SetValue("Distance", DistanceToTarget(SintelPlayer.transform));
        DevelopViewManager.Instance.SetValue("DistanceFromEyes", (SintelPlayer.transform.position - transform.position).magnitude);
    }

    private void OnAnimationEventHandle(int value)
    {
        if (value == (int)AnimationEventId.Attack)
            Attack(SintelPlayer.transform);
    }

    private void OnDrawGizmos()
    {
        if(Agent != null)
        {
            var path = Agent.path;
            if (path != null)
            {
                var corners = path.corners;
                for (int i = 1; i < corners.Length; i++)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(corners[i - 1], corners[i]);
                    Gizmos.DrawSphere(corners[i - 1], 0.3f);
                }
            }

            Gizmos.color = isFollowToTarget ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }

    IEnumerator CorDeath()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
