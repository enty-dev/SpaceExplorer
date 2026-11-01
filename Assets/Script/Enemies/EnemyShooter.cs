using UnityEngine;

/// <summary>
/// Enemy shooter: fires a BulletPattern (ScriptableObject) continuously.
/// Attach this to your enemy prefab along with a BulletPattern asset.
/// 
/// Usage:
/// 1. Create a bullet pattern asset (Right-click > Create > Bullet Patterns > Circle Pattern)
/// 2. Attach EnemyShooter to enemy
/// 3. Assign pattern to "Bullet Pattern" field
/// 4. Set fire rate
/// 5. Check "Auto Fire"
/// </summary>
public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private BulletPattern bulletPattern;
    [SerializeField] private bool autoFire = true;
    [SerializeField] private float fireRate = 0.5f;
    private float fireTimer = 0f;

    private void Update()
    {
        if (!autoFire || bulletPattern == null) return;

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    /// <summary>
    /// Fire the pattern once
    /// </summary>
    public void Fire()
    {
        if (bulletPattern != null)
        {
            bulletPattern.Fire(transform.position);
        }
        else
        {
            Debug.LogWarning("EnemyShooter: No bullet pattern assigned!", this);
        }
    }

    /// <summary>
    /// Start automatic firing
    /// </summary>
    public void StartShooting()
    {
        autoFire = true;
        fireTimer = 0f;
    }

    /// <summary>
    /// Stop automatic firing
    /// </summary>
    public void StopShooting()
    {
        autoFire = false;
    }

    /// <summary>
    /// Set a new pattern
    /// </summary>
    public void SetPattern(BulletPattern newPattern)
    {
        bulletPattern = newPattern;
    }
}
