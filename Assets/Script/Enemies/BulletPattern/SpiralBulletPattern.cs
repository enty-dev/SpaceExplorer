using UnityEngine;

/// <summary>
/// Spiral bullet pattern: fires bullets in a rotating spiral (ScriptableObject)
/// Create asset: Right-click > Create > Spiral Pattern
/// </summary>
[CreateAssetMenu(fileName = "Pattern_Spiral", menuName = "Bullet Patterns/Spiral Pattern")]
public class SpiralBulletPattern : BulletPattern
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount = 12;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float rotationMultiplier = 1f; // Increase this for tighter spirals

    private float spiralRotation = 0f;

    public override void Fire(Vector3 firePosition, float angle = 0f)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("SpiralBulletPattern: bulletPrefab not assigned!");
            return;
        }

        spiralRotation += rotationMultiplier;

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = (360f / bulletCount) * i + spiralRotation + angle;
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
