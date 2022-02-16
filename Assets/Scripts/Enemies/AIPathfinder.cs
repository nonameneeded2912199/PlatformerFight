using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class AIPathfinder : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 5f;

    public float nextWaypointDistance = 3f;

    public float jumpNodeHeightRequirement = 0.8f;

    public float jumpForce = 5f;

    [Header("Behaviors")]
    public bool followEnabled = false;
    public bool jumpEnabled = true;
    public bool flipEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private BaseCharacter character;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        character = GetComponent<BaseCharacter>();

        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (target != null && followEnabled)
        {
            PathFollow();
        }
    }

    public void StartPathFinding(Transform target, float speed)
    {
        this.target = target;
        this.speed = speed;

        followEnabled = true;
    }

    public void StopPathFinding()
    {
        followEnabled = false;
    }

    private void UpdatePath()
    {
        if (followEnabled && target != null)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(transform.position, target.position, OnPathComplete);
            }
        }
    }

    public void StopFinding()
    {
        
    }

    private void PathFollow()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - character.Rigidbody.position).normalized;

        if (jumpEnabled && character.IsGrounded)
        {
            if (direction.y > 0.9f /*&& target.GetComponent<Rigidbody2D>().velocity.y == 0*/ && path.path.Count < 20)
            {
                character.Rigidbody.velocity = new Vector2(character.Rigidbody.velocity.x, jumpForce);
            }
        }

        character.SetVelocity(speed);

        float distance = Vector2.Distance(character.Rigidbody.position, 
            path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (flipEnabled)
        {
            if ((direction.x > 0 && !character.facingRight)
                || (direction.x < 0 && character.facingRight))
                character.Flip();
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

}
