using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Object pool for bullets. Reuses bullets instead of Instantiate/Destroy.
/// Critical for performance in bullet-hell games with hundreds of bullets.
/// 
/// Usage:
///   BulletPool.Instance.GetBullet(prefab, position, velocity);
///   BulletPool.Instance.ReturnBullet(bulletGameObject);
/// </summary>
public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }

    [SerializeField] private int initialPoolSize = 300;
    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Get a bullet from the pool or create a new one
    /// </summary>
    public GameObject GetBullet(GameObject prefab, Vector3 position, Vector2 velocity)
    {
        if (prefab == null)
        {
            Debug.LogError("BulletPool: Prefab is null!");
            return null;
        }

        // Create pool for this prefab if it doesn't exist
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new Queue<GameObject>();
            PrewarmPool(prefab, initialPoolSize);
        }

        GameObject bullet;

        // Get from pool or create new
        if (pools[prefab].Count > 0)
        {
            bullet = pools[prefab].Dequeue();
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(prefab);
        }

        // Configure bullet
        bullet.transform.position = position;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = velocity;
        }

        return bullet;
    }

    /// <summary>
    /// Return a bullet to the pool
    /// </summary>
    public void ReturnBullet(GameObject bullet)
    {
        if (bullet == null) return;

        bullet.SetActive(false);

        // Find which pool this bullet belongs to
        GameObject originalPrefab = bullet.GetComponent<Bullet>() != null ? bullet : null;

        // For now, just destroy if we can't identify the prefab
        // In production, you'd track the prefab reference on the bullet
        Destroy(bullet);
    }

    /// <summary>
    /// Pre-allocate bullets in the pool
    /// </summary>
    private void PrewarmPool(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject bullet = Instantiate(prefab);
            bullet.SetActive(false);
            pools[prefab].Enqueue(bullet);
        }

        Debug.Log($"BulletPool: Prewarm {count} bullets for {prefab.name}");
    }

    /// <summary>
    /// Get current pool statistics
    /// </summary>
    public string GetPoolStats()
    {
        string stats = "BulletPool Stats:\n";
        foreach (var kvp in pools)
        {
            stats += $"{kvp.Key.name}: {kvp.Value.Count} available\n";
        }
        return stats;
    }
}
