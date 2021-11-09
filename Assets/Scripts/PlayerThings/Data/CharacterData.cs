using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCharacterData", menuName = "Characters/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Move State")]
    //public Vector2 standColliderOffset;
    //public Vector2 standColliderSize;
    public float movementVelocity = 10;
    public float maxSlopeAngle = 80f;
    //public float fastMoveMultiplier = 1.5f;

    [Header("Jump State")]
    public float gravityScale = 6;
    public float jumpVelocity = 20;
    public int maxJumpCount = 2;
    public float jumpHeightMultiplier = 0.5f;
    public float maxJumpDelay = 0.2f;

    [Header("Air State")]
    //public float movementInAirMultiplier = 0.8f;
    public float wallSlideThehold = -3f;
    public float maxFallVelocity = -20f;

    [Header("Check Data")]
    //public Vector2 groundCheckBorder = new Vector2(0.1f, 0.3f);
    //public Vector2 wallCheckBorder = new Vector2(0.5f, 0.3f);
    public float groundRaycastLength;
    public Vector3 groundRaycastOffset;
    public float wallRaycastLength;
    public float ceilingCheckRadius = 0.25f;
    public LayerMask platformLayer;

    [Header("Wall Data")]
    public float wallClimbVelocity = 3;
    public float wallSlideVelocity = 5;

    [Header("Wall Jump State")]
    public Vector2 wallJumpVelocity = new Vector2(7, 20);
    public float wallJumpTime = 0.3f;

    [Header("Wall Negative Jump State")]
    public Vector2 wallNegativeJumpVelocity = new Vector2(10, 15);
    public float wallNegativeJumpTime = 0.3f;

    [Header("Scroll State")]
    public float scrollVelocity = 10;
    public Vector2 scrollColliderOffset;
    public Vector2 scrollColliderSize;

    [Header("Climb Ladder State")]
    public float climbLadderVelocity = 5f;
    public float climbLadderCoolTime = 0.3f;

    [Header("Dash State")]
    public Vector2 dashVelocity = new Vector2(20, 10);
    public float dashTime = 0.3f;
    public float dashCoolTime = 2f;

}
