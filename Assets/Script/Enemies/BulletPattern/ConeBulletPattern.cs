using UnityEngine;

/// <summary>
/// Cone/Arc bullet pattern: fires bullets in a fan-shaped cone (ScriptableObject)
/// Create asset: Right-click > Create > Cone Pattern
/// </summary>
[CreateAssetMenu(fileName = "Pattern_Cone", menuName = "Bullet Patterns/Cone Pattern")]
public class ConeBulletPattern : BulletPattern
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount = 7;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float coneAngle = 60f; // Total angle of the cone
    [SerializeField] private float centerAngle = -90f; // Direction of center

    public override void Fire(Vector3 firePosition, float additionalAngle = 0f)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("ConeBulletPattern: bulletPrefab not assigned!");
            return;
        }

        float finalCenterAngle = centerAngle + additionalAngle;
        float minAngle = finalCenterAngle - coneAngle * 0.5f;
        float maxAngle = finalCenterAngle + coneAngle * 0.5f;
        float angleStep = (maxAngle - minAngle) / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = minAngle + angleStep * i;
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
