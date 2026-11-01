/*
 * BULLET PREFAB SETUP GUIDE
 * ===========================
 * 
 * This guide explains how to create and configure bullet prefabs for your bullet patterns.
 * 
 * STEP 1: Create the Bullet Prefab in Unity Editor
 * ===================================================
 * 
 * 1. Create a new GameObject named "Bullet" (or "EnemyBullet")
 * 2. Add these components:
 * 
 *    a) Sprite Renderer
 *       - Assign a bullet sprite (small circle or bullet shape)
 *       - Set Order in Layer: 5 (above enemies)
 * 
 *    b) Circle Collider 2D
 *       - Set Radius: 0.1 (adjust for your sprite size)
 *       - Check "Is Trigger": TRUE
 * 
 *    c) Rigidbody 2D
 *       - Body Type: Dynamic
 *       - Gravity Scale: 0
 *       - Collision Detection: Continuous
 *       - Constraints:
 *         - Freeze Rotation Z: TRUE
 *         - Freeze Z Position: TRUE
 * 
 * 3. Create a C# script called "Bullet" and attach it (see below)
 * 
 * 4. Drag the GameObject into Assets/Resources/Prefabs/Bullets/ folder
 * 
 * 5. Delete the GameObject from the scene
 * 
 * 
 * STEP 2: Create Multiple Bullet Types (Optional)
 * ================================================
 * 
 * You can create different bullet types:
 * - EnemyBullet_Small (fast, weak)
 * - EnemyBullet_Medium (normal)
 * - EnemyBullet_Large (slow, dangerous)
 * 
 * Just create different sprites and prefabs with the same script.
 * 
 * 
 * STEP 3: Assign Prefabs to Bullet Patterns
 * ==========================================
 * 
 * In Unity Inspector:
 * 1. Create a ScriptableObject for each pattern type
 * 2. Right-click in Project > Create > CircleBulletPattern (etc)
 * 3. Assign the Bullet prefab to the "Bullet Prefab" field
 * 4. Adjust pattern parameters:
 *    - Bullet Count
 *    - Bullet Speed
 *    - Spread Angle (if applicable)
 * 5. Save the ScriptableObject (e.g., "Pattern_Circle_Basic")
 * 
 * 
 * EXAMPLE FOLDER STRUCTURE
 * ========================
 * 
 * Assets/
 * ├── Resources/
 * │   └── Prefabs/
 * │       └── Bullets/
 * │           ├── EnemyBullet_Small.prefab
 * │           ├── EnemyBullet_Medium.prefab
 * │           └── EnemyBullet_Large.prefab
 * │
 * ├── ScriptableObjects/
 * │   └── BulletPatterns/
 * │       ├── Pattern_Circle_Basic.asset
 * │       ├── Pattern_Spiral_Fast.asset
 * │       ├── Pattern_Wave_Medium.asset
 * │       └── Pattern_Cone_Spread.asset
 * │
 * └── Script/
 *     └── Enemies/
 *         ├── BulletPattern/
 *         │   ├── BulletPattern.cs
 *         │   ├── CircleBulletPattern.cs
 *         │   ├── SpiralBulletPattern.cs
 *         │   ├── etc...
 *         │
 *         └── Bullet/
 *             └── Bullet.cs (see below)
 * 
 * 
 * RECOMMENDED BULLET PREFAB SIZE/SPEED
 * =====================================
 * 
 * Size:
 * - Sprite size: 0.2 x 0.2 units (small)
 * - Collider radius: 0.1 units
 * 
 * Speed:
 * - Slow: 3-4 units/second
 * - Normal: 5-6 units/second
 * - Fast: 7-8 units/second
 * - Very Fast: 9-10+ units/second
 * 
 * 
 * COLLISION LAYERS
 * ================
 * 
 * Set up layers in Unity for proper collision:
 * 1. Create layers: "Enemy", "EnemyBullet", "Player", "PlayerBullet"
 * 2. Set bullet prefab layer to "EnemyBullet"
 * 3. Configure Physics2D collision matrix:
 *    - EnemyBullet should collide with: Player
 *    - PlayerBullet should collide with: Enemy
 * 
 * 
 * PERFORMANCE OPTIMIZATION
 * ========================
 * 
 * For hundreds of bullets (Touhou-style), implement Object Pooling:
 * 
 * 1. Create a BulletPool script (Object Pool pattern)
 * 2. Pre-instantiate 500+ bullets at game start
 * 3. Reuse bullets instead of Instantiate/Destroy
 * 4. This reduces GC pressure significantly
 * 
 * Example pooling approach:
 * - Create bullets on demand up to max count
 * - Return bullets to pool when off-screen
 * - Reset velocity/position when reused
 * 
 */

/*
 * ACTUAL BULLET SCRIPT IS IN: Assets/Script/Enemies/Bullet/Bullet.cs
 * The Bullet.cs file contains the MonoBehaviour script to attach to your prefab.
 * 
 * THIS FILE (BulletPrefabGuide.cs) IS JUST A REFERENCE GUIDE - NO CODE TO RUN
 * Delete it if you don't need the documentation.
 */
