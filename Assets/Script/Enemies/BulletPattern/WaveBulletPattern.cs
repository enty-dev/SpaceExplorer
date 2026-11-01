using UnityEngine;

/// <summary>
/// Wave bullet pattern: fires bullets in a wave formation (ScriptableObject)
/// Create asset: Right-click > Create > Wave Pattern
/// </summary>
[CreateAssetMenu(fileName = "Pattern_Wave", menuName = "Bullet Patterns/Wave Pattern")]
public class WaveBulletPattern : BulletPattern
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletsPerWave = 8;
    [SerializeField] private float waveHeight = 2f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float angle = -90f; // Direction of travel

    private float wavePhase = 0f;

    public override void Fire(Vector3 firePosition, float additionalAngle = 0f)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("WaveBulletPattern: bulletPrefab not assigned!");
            return;
        }

        wavePhase += 45f; // Rotate wave each fire

        float finalAngle = angle + additionalAngle;
        Vector3 forwardDirection = new Vector3(Mathf.Cos(finalAngle * Mathf.Deg2Rad), 
                                             Mathf.Sin(finalAngle * Mathf.Deg2Rad), 0);
        Vector3 rightDirection = new Vector3(-forwardDirection.y, forwardDirection.x, 0);

        for (int i = 0; i < bulletsPerWave; i++)
        {
            float waveOffset = Mathf.Sin((i / (float)bulletsPerWave) * Mathf.PI + wavePhase * Mathf.Deg2Rad) * waveHeight;
            Vector3 bulletPos = firePosition + rightDirection * waveOffset;
            Quaternion rotation = Quaternion.Euler(0, 0, finalAngle);

            GameObject bullet = Instantiate(bulletPrefab, bulletPos, rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = forwardDirection * bulletSpeed;
            }
        }
    }
}
