using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevParams : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] ForceMode jumpForceMode = ForceMode.Force;

    public float MoveSpeed => moveSpeed;
    public ForceMode JumpForceMode => jumpForceMode;
    public float JumpForce => jumpForce;
}
