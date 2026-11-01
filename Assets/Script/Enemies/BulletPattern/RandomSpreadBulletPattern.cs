using UnityEngine;

/// <summary>
/// Random spread bullet pattern: fires bullets in random directions (ScriptableObject)
/// Create asset: Right-click > Create > Random Spread Pattern
/// </summary>
[CreateAssetMenu(fileName = "Pattern_RandomSpread", menuName = "Bullet Patterns/Random Spread Pattern")]
public class RandomSpreadBulletPattern : BulletPattern
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount = 10;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float spreadAngle = 360f; // Spread angle range

    public override void Fire(Vector3 firePosition, float centerAngle = 0f)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("RandomSpreadBulletPattern: bulletPrefab not assigned!");
            return;
        }

        float minAngle = centerAngle - spreadAngle * 0.5f;
        float maxAngle = centerAngle + spreadAngle * 0.5f;

        for (int i = 0; i < bulletCount; i++)
        {
            float randomAngle = Random.Range(minAngle, maxAngle);
            Quaternion rotation = Quaternion.Euler(0, 0, randomAngle);

            GameObject bullet = Instantiate(bulletPrefab, firePosition, rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector3 direction = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 
                                              Mathf.Sin(randomAngle * Mathf.Deg2Rad), 0);
                rb.linearVelocity = direction * bulletSpeed;
            }
        }
    }
}
