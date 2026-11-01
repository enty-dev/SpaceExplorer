using UnityEngine;

/// <summary>
/// Test script for bullet patterns.
/// Attach this to your enemy prefab to test firing patterns with keyboard input.
/// 
/// Usage:
/// 1. Attach this to enemy GameObject
/// 2. In Inspector, assign a BulletPattern
/// 3. Press SPACE to fire the pattern
/// 4. Watch bullets spawn and fly
/// </summary>
public class BulletPatternTest : MonoBehaviour
{
    [SerializeField] private BulletPattern bulletPattern;
    [SerializeField] private KeyCode fireKey = KeyCode.Space;
    [SerializeField] private float fireRate = 0.2f;
    private float fireTimer = 0f;

    private void Update()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetKey(fireKey) && fireTimer >= fireRate)
        {
            if (bulletPattern != null)
            {
                bulletPattern.Fire(transform.position);
                fireTimer = 0f;
            }
            else
            {
                Debug.LogWarning("BulletPatternTest: No bullet pattern assigned!");
            }
        }
    }

    [ContextMenu("Test Pattern Once")]
    public void TestPatternOnce()
    {
        if (bulletPattern != null)
        {
            bulletPattern.Fire(transform.position);
            Debug.Log($"Fired pattern: {bulletPattern.GetPatternName()}");
        }
        else
        {
            Debug.LogWarning("No bullet pattern assigned!");
        }
    }
}
