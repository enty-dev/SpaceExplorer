using UnityEngine;

/// <summary>
/// Aimed bullet pattern: fires bullets towards the player
/// Create as asset: Right-click > Create > Aim Pattern
/// </summary>
[CreateAssetMenu(fileName = "Pattern_Aim", menuName = "Bullet Patterns/Aim Pattern")]
public class AimBulletPattern : BulletPattern
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount = 5;
    [SerializeField] private float bulletSpeed = 6f;
    [SerializeField] private float spreadAngle = 30f; // Spread around aim direction

    private Transform playerTransform;

    /// <summary>
    /// Set the player target (call this from enemy on Start)
    /// </summary>
    public void SetPlayerTarget(Transform player)
    {
        playerTransform = player;
    }

    public override void Fire(Vector3 firePosition, float angle = 0f)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("AimBulletPattern: bulletPrefab not assigned!");
            return;
        }

        // Calculate aim direction towards player
        Vector3 aimDirection = Vector3.down; // Default direction if no player

        if (playerTransform != null)
        {
            aimDirection = (playerTransform.position - firePosition).normalized;
        }

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        float minAngle = aimAngle - spreadAngle * 0.5f;
        float maxAngle = aimAngle + spreadAngle * 0.5f;
        float angleStep = bulletCount > 1 ? (maxAngle - minAngle) / (bulletCount - 1) : 0;

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = minAngle + angleStep * i + angle;
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);

            GameObject bullet = Instantiate(bulletPrefab, firePosition, rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector3 direction = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), 
                                              Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0);
                rb.linearVelocity = direction * bulletSpeed;
            }
        }
    }
}
