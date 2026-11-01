using UnityEngine;

/// <summary>
/// Enemy bullet script. Attach this to enemy bullet prefab.
/// When it hits the player, the player loses 1 life.
/// No damage system - just instant life loss.
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 15f; // Destroy if off-screen too long
    private float spawnTime;
    private Rigidbody2D rb;

    private void Start()
    {
        spawnTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Rotate bullet to face direction of travel
        if (rb != null && rb.linearVelocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // Destroy if bullet is off-screen for too long
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If bullet hits player hitbox, notify it
        if (collision.CompareTag("PlayerHitbox"))
        {
            // PlayerHitbox will handle the collision response
            // Just destroy the bullet
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Call this to set bullet velocity (used by patterns)
    /// </summary>
    public void SetVelocity(Vector2 velocity)
    {
        if (rb != null)
            rb.linearVelocity = velocity;
    }
}
