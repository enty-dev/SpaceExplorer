using UnityEngine;

/// <summary>
/// QUICK BULLET PREFAB SETUP - Follow these steps:
/// 
/// STEP 1: CREATE THE PREFAB
/// =========================
/// 1. In Unity, create a new GameObject → name it "EnemyBullet"
/// 2. Add Component: Sprite Renderer
///    - Drag a bullet sprite into "Sprite" field
///    - Set "Order in Layer" to 5
/// 
/// 3. Add Component: Circle Collider 2D
///    - Set "Radius" to 0.1
///    - CHECK "Is Trigger"
/// 
/// 4. Add Component: Rigidbody 2D
///    - Body Type: Dynamic
///    - Gravity Scale: 0
///    - Collision Detection: Continuous
///    - Constraints: Freeze Rotation Z ✓ and Freeze Z Position ✓
/// 
/// 5. Add Component: Bullet (the script from Bullet.cs)
/// 
/// 6. Drag into: Assets/Resources/Prefabs/Bullets/EnemyBullet.prefab
/// 
/// 
/// STEP 2: CREATE BULLET PATTERN ASSETS
/// ====================================
/// In Project window, right-click:
/// Create > CircleBulletPattern
/// 
/// In Inspector:
/// - Assign Bullet Prefab: EnemyBullet (drag from Projects folder)
/// - Bullet Count: 8
/// - Bullet Speed: 5
/// 
/// Save it as: Assets/ScriptableObjects/BulletPatterns/Pattern_Circle_Basic.asset
/// 
/// 
/// STEP 3: TEST IT
/// ===============
/// Create test enemy:
/// 
/// GameObject testEnemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
/// EnemyScriptRunner runner = testEnemy.AddComponent<EnemyScriptRunner>();
/// runner.AddAction(new ShootAction(circleBulletPattern, duration: 2f, fireRate: 0.2f))
///        .Run();
/// 
/// See bullets fly in circle!
/// 
/// 
/// PARAMETERS TO TWEAK
/// ===================
/// 
/// Bullet Size:
/// - Small, fast: 0.15 x 0.15
/// - Medium: 0.2 x 0.2
/// - Large, slow: 0.3 x 0.3
/// 
/// Bullet Speed:
/// - Slow (player has time): 3-4
/// - Normal (challenging): 5-6
/// - Fast (intense): 7-8
/// - Very Fast (frantic): 9+
/// 
/// Bullet Lifetime:
/// - Short: 10 seconds (bullets disappear quickly)
/// - Normal: 15 seconds
/// - Long: 20+ seconds (for slow projectiles)
/// 
/// Collider Radius:
/// - Tight hitbox: 0.05
/// - Normal: 0.1
/// - Generous (easier to dodge): 0.15
/// 
/// </summary>
[ExecuteAlways] // This makes it visible in inspector at all times
public class BulletSetupVisualGuide : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("═══ BULLET PREFAB SETUP GUIDE ═══")]
    [TextArea(15, 20)]
    [SerializeField] private string setupInstructions = 
        "STEP 1: Create GameObject 'EnemyBullet'\n" +
        "STEP 2: Add Sprite Renderer (assign bullet sprite)\n" +
        "STEP 3: Add Circle Collider 2D (radius: 0.1, Is Trigger: ✓)\n" +
        "STEP 4: Add Rigidbody 2D (Body Type: Dynamic, Gravity: 0)\n" +
        "STEP 5: Add Bullet script\n" +
        "STEP 6: Save as prefab in Assets/Resources/Prefabs/Bullets/\n" +
        "\n" +
        "STEP 7: Create bullet pattern ScriptableObject\n" +
        "Right-click > Create > CircleBulletPattern\n" +
        "Assign prefab and tune parameters\n";

    [Header("Quick Parameter Reference")]
    [SerializeField] private float recommendedBulletSpeed = 5f;
    [SerializeField] private float recommendedColliderRadius = 0.1f;
    [SerializeField] private int recommendedBulletCount = 8;
    [SerializeField] private float recommendedFireRate = 0.2f;

    [ContextMenu("Show Setup Steps")]
    public void ShowSetupSteps()
    {
        Debug.Log(
            "=== BULLET SETUP ===\n" +
            "1. Create GameObject 'EnemyBullet'\n" +
            "2. Components needed:\n" +
            "   - Sprite Renderer\n" +
            "   - Circle Collider 2D (Is Trigger)\n" +
            "   - Rigidbody 2D\n" +
            "   - Bullet script\n" +
            "3. Save as prefab\n" +
            "4. Create BulletPattern ScriptableObjects\n" +
            "5. Use in ShootAction with EnemyScriptRunner"
        );
    }
#endif
}

// THIS IS JUST A GUIDE - You don't need to use this MonoBehaviour in your game
// It's just showing instructions. Delete this file if you prefer!
