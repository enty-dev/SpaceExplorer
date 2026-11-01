using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    //Playyer property
    public float moveSpeed = 7f;
    public float moveSpeedSlow = 4f;
    public float boundaryMargin = 0.1f; // Distance from screen edge to clamp player
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float minX, maxX, minY, maxY;


    //Laser property
    public Camera mainCamera;
    public float maxDistance = 100f;
    public float gameplayRightBoundaryX = 6.5f; // Right edge of gameplay area (where sidebar starts)

    public LayerMask hitMask;



    public float damagePerSecond;

    //Particle property


    // Damage/Invulnerability property
    private bool isInvulnerable = false;
    [SerializeField] private float invulnerabilityDuration = 3f;
    [SerializeField] private float blinkInterval = 0.1f;

    // Player health
    [SerializeField] private int currentLives = 3;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip collisionSound;

    // Hitbox reference
    private PlayerHitbox playerHitbox;

    private bool hasUnlockedSideLasers = false; //flag to track if side lasers are unlocked


    private float shiftLerpT = 0f;  // 0 = spread, 1 = tight



    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        mainCamera = Camera.main;

        // Get PlayerHitbox reference
        playerHitbox = GetComponentInChildren<PlayerHitbox>();
        if (playerHitbox == null)
        {
            Debug.LogWarning("PlayerScript: PlayerHitbox not found in children!");
        }

        // Calculate camera bounds and apply margin
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        minX = mainCamera.transform.position.x - camWidth + boundaryMargin;
        maxX = mainCamera.transform.position.x + camWidth - boundaryMargin;
        minY = mainCamera.transform.position.y - camHeight + boundaryMargin;
        maxY = mainCamera.transform.position.y + camHeight - boundaryMargin;

        // Log boundary values for debugging
        Debug.Log($"Boundary - MinX: {minX}, MaxX: {maxX}, MinY: {minY}, MaxY: {maxY}");


    }

    void Update()
    {

        if (!hasUnlockedSideLasers && damagePerSecond >= 800)
        {
    
            hasUnlockedSideLasers = true;

 
        }

        var keyboard = Keyboard.current;
        moveInput = Vector2.zero;

        if (keyboard.leftArrowKey.isPressed) moveInput.x = -1;
        if (keyboard.rightArrowKey.isPressed) moveInput.x = 1;
        if (keyboard.upArrowKey.isPressed) moveInput.y = 1;
        if (keyboard.downArrowKey.isPressed) moveInput.y = -1;

        moveInput = moveInput.normalized;



    }

    void FixedUpdate()
    {
        var keyboard = Keyboard.current;
        bool isShiftHeld = keyboard.shiftKey.isPressed;

        // Movement
        float currentMoveSpeed = isShiftHeld ? moveSpeedSlow : moveSpeed;
        rb.linearVelocity = moveInput * currentMoveSpeed;

     


    }

    void ApplyDamage(Collider2D collider)
    {
        float damage = damagePerSecond * Time.deltaTime;


    }

    /// <summary>
    /// Called by PlayerHitbox when it detects a collision with an enemy/asteroid/bullet.
    /// Handles taking damage and triggering invulnerability.
    /// </summary>
    public void TakeDamage()
    {
        Debug.Log($"[PlayerScript] TakeDamage called. isInvulnerable: {isInvulnerable}, currentLives: {currentLives}");

        if (isInvulnerable) 
        {
            Debug.Log("[PlayerScript] Ignoring damage - player is invulnerable");
            return;
        }

        currentLives--;
        Debug.Log($"[PlayerScript] Player took damage! Lives remaining: {currentLives}");
        UpdateHeartsUI();

        if (currentLives <= 0)
        {
            Debug.Log("[PlayerScript] Game Over - no lives left!");
            if (explosionPrefab != null)
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            if (collisionSound != null)
                AudioManager.PlayClip(collisionSound, transform.position);

            GameManager.Instance.HandleGameOver();
            Destroy(gameObject);
        }
        else
        {
            // Start invulnerability blink BEFORE notifying GameManager
            // This ensures the coroutine runs before SetActive(false)
            StartCoroutine(BlinkDuringInvulnerability());

            // Play sound and effects immediately
            if (collisionSound != null)
                AudioManager.PlayClip(collisionSound, transform.position);

            if (explosionPrefab != null)
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Only disable player after invulnerability duration (removed GameManager call)
            // GameManager.Instance.HandlePlayerCollision() was disabling player too early
        }
    }

    /// <summary>
    /// Disables laser while player is invulnerable
    /// </summary>
    private void DisableLaser()
    {
        // TODO: Implement laser disable logic
    }

    /// <summary>
    /// Updates hearts UI based on current lives
    /// </summary>
    private void UpdateHeartsUI()
    {
        // TODO: Implement hearts UI update logic
    }

    public void IncreaseDamage(float amount)
    {
        if (damagePerSecond == 1000)
        {
            return;
        }

        damagePerSecond += amount;

        Debug.Log("Updated Damage Per Second: " + damagePerSecond);
    }



    public IEnumerator BlinkDuringInvulnerability()
    {
        isInvulnerable = true;
        Debug.Log($"[PlayerScript] Blink started! Duration: {invulnerabilityDuration}s, Interval: {blinkInterval}s");

        DisableLaser();

        float elapsed = 0f;
        List<SpriteRenderer> allRenderers = new List<SpriteRenderer>();

        // Collect all SpriteRenderers (ShipVisual + Weapon children)
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>(true))
        {
            allRenderers.Add(sr);
        }

        Debug.Log($"[PlayerScript] Found {allRenderers.Count} SpriteRenderers to blink");

        while (elapsed < invulnerabilityDuration)
        {
            foreach (var sr in allRenderers)
            {
                sr.enabled = false;
            }

            yield return new WaitForSeconds(blinkInterval);

            foreach (var sr in allRenderers)
            {
                sr.enabled = true;
            }

            yield return new WaitForSeconds(blinkInterval);

            elapsed += 2f * blinkInterval;
        }

        // Ensure all renderers are visible after invulnerability ends
        foreach (var sr in allRenderers)
        {
            sr.enabled = true;
        }

        Debug.Log("[PlayerScript] Blink ended! Re-enabling hitbox and laser");
        Debug.Log($"[PlayerScript] Setting isInvulnerable = false");

        // Re-enable laser and hitbox
        Debug.Log("[PlayerScript] About to call EnableLaser()");
        EnableLaser();
        Debug.Log("[PlayerScript] EnableLaser() called");

        isInvulnerable = false;
        Debug.Log($"[PlayerScript] isInvulnerable is now: {isInvulnerable}");
    }

    /// <summary>
    /// Re-enables laser after invulnerability ends
    /// </summary>
    private void EnableLaser()
    {
        // Re-enable hitbox
        if (playerHitbox != null)
        {
            playerHitbox.EnableHitbox();
        }
    }
}
