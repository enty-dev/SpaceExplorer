using UnityEngine;

/// <summary>
/// Base enemy script: handles health and basic properties.
/// For shooting, attach EnemyShooter and a BulletPattern asset.
/// For complex behaviors, use EnemyScriptRunner.
/// </summary>
public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHp = 10;
    protected int currentHp;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public Vector3 targetPosition = Vector3.zero;

    protected virtual void Start()
    {
        currentHp = maxHp;
        if (targetPosition == Vector3.zero)
        {
            targetPosition = transform.position;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
