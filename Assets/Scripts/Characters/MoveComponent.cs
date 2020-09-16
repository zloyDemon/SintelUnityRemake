using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveComponent : MonoBehaviour
{
    private Rigidbody rigidBody;

    public bool IsMoving { get; private set; }
    public float MoveSpeed { get; private set; }
    public float RotationSpeed { get; private set; }
    public float JumpForcce { get; private set; }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
    }

    public void Move(Vector3 direction)
    {
        rigidBody.MovePosition(transform.position + Vector3.ClampMagnitude(direction, 1f) * MoveSpeed * Time.fixedDeltaTime);
        IsMoving = direction != Vector3.zero && MoveSpeed != 0;
    }

    public Quaternion RotateTo(Vector3 direction)
    {
        direction.y = 0;
        Quaternion directionRotation = Quaternion.LookRotation(direction, Vector3.up);
        var result = Quaternion.Slerp(transform.rotation, directionRotation, RotationSpeed * Time.deltaTime);
        transform.rotation = result;
        return result;
    }

    public Quaternion RotateToTarget(Transform target)
    {
        var direction = (target.position - transform.position).normalized;
        return RotateTo(direction);
    }

    public void MoveWithRotation(Vector3 direction)
    {
        Move(direction);
        RotateTo(direction);
    }

    public void Jump()
    {
        rigidBody.AddForce(Vector3.up * (JumpForcce * 100) * Time.fixedDeltaTime, ForceMode.Force);
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        MoveSpeed = moveSpeed;
    }

    public void SetRotationSpeed(float rotationSpeed)
    {
        RotationSpeed = rotationSpeed;
    }

    public void SetJumpForce(float jumpForce)
    {
        JumpForcce = jumpForce;
    }

    public void AddForce(Vector3 force, ForceMode mode)
    {
        rigidBody.AddForce(force, mode);
    }
}
