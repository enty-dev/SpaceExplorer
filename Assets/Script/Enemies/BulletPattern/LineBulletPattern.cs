using UnityEngine;

/// <summary>
/// Line bullet pattern: fires bullets in a straight line (ScriptableObject)
/// Create asset: Right-click > Create > Line Pattern
/// </summary>
[CreateAssetMenu(fileName = "Pattern_Line", menuName = "Bullet Patterns/Line Pattern")]
public class LineBulletPattern : BulletPattern
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount = 5;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float spacing = 0.5f;
    [SerializeField] private float angle = -90f; // Direction of fire

    public override void Fire(Vector3 firePosition, float additionalAngle = 0f)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("LineBulletPattern: bulletPrefab not assigned!");
            return;
        }

        float finalAngle = angle + additionalAngle;
        Vector3 direction = new Vector3(Mathf.Cos(finalAngle * Mathf.Deg2Rad), 
                                      Mathf.Sin(finalAngle * Mathf.Deg2Rad), 0);

        // Offset to center the line
        Vector3 offsetStart = firePosition - direction * spacing * (bulletCount - 1) * 0.5f;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 bulletPos = offsetStart + direction * spacing * i;
            Quaternion rotation = Quaternion.Euler(0, 0, finalAngle);

            GameObject bullet = Instantiate(bulletPrefab, bulletPos, rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * bulletSpeed;
            }
        }
    }
}
