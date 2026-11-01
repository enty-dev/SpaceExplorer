using UnityEngine;

/// <summary>
/// Circle bullet pattern: fires bullets in all directions equally
/// Create as asset: Right-click > Create > Circle Pattern
/// </summary>
[CreateAssetMenu(fileName = "Pattern_Circle", menuName = "Bullet Patterns/Circle Pattern")]
public class CirclePatternScript : BulletPattern
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount = 8;
    [SerializeField] private float bulletSpeed = 5f;

    public override void Fire(Vector3 firePosition, float angle = 0f)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("CirclePatternScript: bulletPrefab not assigned!");
            return;
        }

        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = (i * angleStep) + angle;
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);

            GameObject bullet = Instantiate(bulletPrefab, firePosition, rotation);

            // Set bullet velocity
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
    

