using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SintelPlayer : MonoBehaviour
{
    private const float WalkSpeed = 2f;
    private const float RunSpeed = WalkSpeed * 2;
    private const float JumpForce = 100f;
    private const float RotationSpeed = 200;
    private const int BattleStateTime = 8;

    private SintelInput sintelInput;
    private MoveComponent moveComponent;
    private Vector3 currentDirection;
    private SintelAnimatorController animatorController;
    private SintelMoveState currentMoveState;
    private SintelCharacterState currentCharacterState;
    private SintelAttackComponent attackComponent;
    private Rigidbody rigidbody;
    private Coroutine corAttackTimer;
    private float yVelocity;
    private RaycastHit hitInfo;
    private bool oldIsGround;
    private bool isGround;
    private Transform mainCamera;

    public enum SintelMoveState
    {
        Idle,
        Walk,
        Run,
        Fall,
    };

    public enum SintelCharacterState
    {
        Normal,
        Battle,
    }

    private void Awake()
    {
        sintelInput = GetComponent<SintelInput>();
        moveComponent = GetComponent<MoveComponent>();
        animatorController = GetComponent<SintelAnimatorController>();
        attackComponent = GetComponent<SintelAttackComponent>();
        rigidbody = GetComponent<Rigidbody>();
        attackComponent.AttackEvent += OnAttack;
        currentMoveState = SintelMoveState.Idle;
        moveComponent.SetMoveSpeed(WalkSpeed);
        moveComponent.SetRotationSpeed(RotationSpeed);
        moveComponent.SetJumpForce(JumpForce);
        mainCamera = SintelGameManager.Instance.MainCamera.transform;
        animatorController.AnimationEvent += OnAnimationEventHandler;
    }

    private void OnDestroy()
    {
        animatorController.AnimationEvent -= OnAnimationEventHandler;
    }

    private void Update()
    {
        CheckRaycastHit();
        DevelopViewManager.Instance.SetValue("SintelPosition", transform.position.ToString());
    }

    private void FixedUpdate()
    {
        var x = sintelInput.GetInput<float>(SintelInput.SintelInputType.Horizontal);
        var z = sintelInput.GetInput<float>(SintelInput.SintelInputType.Vertical);
        var inputVector = new Vector3(x, 0, z);
        if (inputVector != Vector3.zero)
        { 
            currentDirection = inputVector;
            currentDirection = mainCamera.TransformDirection(currentDirection);

            if (sintelInput.GetInput<bool>(SintelInput.SintelInputType.Sprint) && isGround)
            {
                moveComponent.SetMoveSpeed(RunSpeed);
                currentMoveState = SintelMoveState.Run;
            }
            else
            {
                moveComponent.SetMoveSpeed(WalkSpeed);
                currentMoveState = SintelMoveState.Walk;
            }

            moveComponent.RotateTo(currentDirection);

            currentDirection.y = yVelocity;

            if (!attackComponent.IsAttacking)
                moveComponent.Move(currentDirection);

            Debug.DrawRay(transform.position, currentDirection, Color.green);
        }
        else
        {   
            currentMoveState = SintelMoveState.Idle;
        }


        if (sintelInput.JumpPressed)
            moveComponent.Jump();

        if (sintelInput.GetInput<bool>(SintelInput.SintelInputType.Attack))
            Attack();

        if (!isGround)
            ApplyGravity();
            
        animatorController.SetValue(SintelGameParameters.MoveStateId, (int)currentMoveState);
        animatorController.SetValue(SintelGameParameters.CharacterState, (int)currentCharacterState);
        sintelInput.ResetInputs();
    }

    private void OnAnimationEventHandler(int value)
    {

    }

    private void Attack()
    {
        currentCharacterState = SintelCharacterState.Battle;
        attackComponent.Attack();
        SintelUtils.KillAndNullCoroutine(this, ref corAttackTimer);
        corAttackTimer = StartCoroutine(CorBattleStateTimer());
    }

    private void OnAttack(int attackId)
    {
        Debug.Log($"Attack {attackId}");
    }

    private void CheckRaycastHit()
    {
        bool isRaycasting = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hitInfo, 0.5f);
        isGround = isRaycasting;

        if (oldIsGround != isGround)
        {
            oldIsGround = isGround;
            animatorController.SetValue(SintelGameParameters.AnimatorParameters.IsGround, isGround);
        }

        yVelocity = rigidbody.velocity.y;

        if (isRaycasting)
        {
            if (hitInfo.normal != Vector3.up)
            {
                var crossVector = Vector3.Cross(transform.right, hitInfo.normal).normalized;
                yVelocity = crossVector.y;
            }
        }
    }

    private void ApplyGravity()
    {
        rigidbody.AddForce(Vector3.down * 9.8f * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    IEnumerator CorBattleStateTimer()
    {
        yield return new WaitForSeconds(BattleStateTime);
        currentCharacterState = SintelCharacterState.Normal;
        animatorController.SetValue(SintelGameParameters.AnimatorParameters.CharacterState, (int)currentCharacterState);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGround ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
    }
}