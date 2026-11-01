using UnityEngine;

/// <summary>
/// Moves enemy off-screen and destroys it
/// </summary>
public class LeaveScreenAction : EnemyAction
{
    private float exitSpeed;
    private Vector3 exitDirection;

    public LeaveScreenAction(float speed = 8f, Vector3? direction = null)
    {
        exitSpeed = speed;
        exitDirection = direction ?? Vector3.down; // Default: move down
    }

    public override void Initialize()
    {
        // Move to a position far off-screen
    }

    public override bool Execute()
    {
        transform.position += exitDirection.normalized * exitSpeed * Time.deltaTime;

        // Destroy once we're far enough away
        if (transform.position.y < -15f || transform.position.y > 15f || 
            transform.position.x < -15f || transform.position.x > 15f)
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
