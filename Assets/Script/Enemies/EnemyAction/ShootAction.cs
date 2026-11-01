using UnityEngine;

/// <summary>
/// Fires a bullet pattern for a specified duration
/// </summary>
public class ShootAction : EnemyAction
{
    private BulletPattern bulletPattern;
    private float duration;
    private float fireRate;
    private float elapsedTime;
    private float fireTimer;

    public ShootAction(BulletPattern pattern, float shootDuration, float rateOfFire = 0.5f)
    {
        bulletPattern = pattern;
        duration = shootDuration;
        fireRate = rateOfFire;
    }

    public override void Initialize()
    {
        elapsedTime = 0f;
        fireTimer = 0f;
    }

    public override bool Execute()
    {
        elapsedTime += Time.deltaTime;
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate && bulletPattern != null)
        {
            bulletPattern.Fire(transform.position);
            fireTimer = 0f;
        }

        return elapsedTime >= duration;
    }

    public override void OnCancel()
    {
        // Stop firing if action is cancelled
    }
}
