using UnityEngine;

/// <summary>
/// PlayerHitbox: Touhou-style hitbox system
/// This is a small, separate collider used for detecting enemy bullet hits.
/// When hit, it notifies the player to take damage or respawn.
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    private Rigidbody2D rb;
    private Collider2D hitboxCollider;

    private void Start()
    {
        // Get PlayerScript if not already assigned
        if (playerScript == null)
        {
            playerScript = GetComponentInParent<PlayerScript>();
            if (playerScript == null)
            {
                playerScript = FindAnyObjectByType<PlayerScript>();
                Debug.LogWarning("PlayerHitbox: Had to search for PlayerScript. Consider assigning it in the Inspector.");
            }
        }

        rb = GetComponent<Rigidbody2D>();
        hitboxCollider = GetComponent<Collider2D>();

        // Configure Rigidbody2D for hitbox (should be kinematic or dynamic, no gravity)
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.isKinematic = true; // Hitbox shouldn't be affected by physics
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // Ensure collider is set as trigger
        if (hitboxCollider != null)
        {
            hitboxCollider.isTrigger = true;
        }

        // Tag this GameObject
        gameObject.tag = "PlayerHitbox";
    }

    /// <summary>
    /// Called when a bullet/asteroid/enemy enters the hitbox trigger
    /// Checks tag and notifies PlayerScript to handle damage
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[PlayerHitbox] Collision detected with: {collision.gameObject.name}, Tag: {collision.tag}");

        // Check if the collision is with a damaging object (Asteroid, Bullet, Enemy, etc.)
        if (collision.CompareTag("Asteroid") ||
            collision.CompareTag("Bullet") ||
            collision.CompareTag("BossMap3") ||
            collision.CompareTag("Shield") ||
            collision.CompareTag("Enemy") ||
            collision.CompareTag("Enemy2") ||
            collision.CompareTag("Enemy2_2"))
        {
            Debug.Log($"[PlayerHitbox] Valid collision! Calling TakeDamage()");

            // Notify player to take damage
            if (playerScript != null)
            {
                playerScript.TakeDamage();

                // Disable hitbox to prevent multiple hits during invulnerability
                DisableHitbox();
            }
            else
            {
                Debug.LogError("PlayerHitbox: PlayerScript reference is null! Cannot apply damage.");
            }
        }
        else
        {
            Debug.Log($"[PlayerHitbox] Collision ignored - tag '{collision.tag}' not in damage list");
        }
    }

    /// <summary>
    /// Disables this hitbox temporarily
    /// </summary>
    public void DisableHitbox()
    {
        Debug.Log("[PlayerHitbox] Disabling hitbox collider");
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
            Debug.Log($"[PlayerHitbox] Hitbox collider disabled: {!hitboxCollider.enabled}");
        }
    }

    /// <summary>
    /// Re-enables this hitbox (called by PlayerScript after invulnerability ends)
    /// </summary>
    public void EnableHitbox()
    {
        Debug.Log("[PlayerHitbox] Enabling hitbox collider");
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = true;
            Debug.Log($"[PlayerHitbox] Hitbox collider enabled: {hitboxCollider.enabled}");
        }
        else
        {
            Debug.LogError("[PlayerHitbox] hitboxCollider is null!");
        }
    }
}


