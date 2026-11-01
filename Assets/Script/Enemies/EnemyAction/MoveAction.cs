using UnityEngine;

/// <summary>
/// Moves an enemy to a target position
/// </summary>
public class MoveAction : EnemyAction
{
    private Vector3 targetPosition;
    private float moveSpeed;
    private float stoppingDistance;

    public MoveAction(Vector3 target, float speed = 5f, float stopDist = 0.1f)
    {
        targetPosition = target;
        moveSpeed = speed;
        stoppingDistance = stopDist;
    }

    public override void Initialize()
    {
        // No setup needed
    }

    public override bool Execute()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance <= stoppingDistance)
        {
            transform.position = targetPosition;
            return true; // Action complete
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
        return false; // Still moving
    }
}
