using UnityEngine;

/// <summary>
/// Base class for bullet patterns.
/// Different patterns can be created by subclassing.
/// </summary>
public abstract class BulletPattern : ScriptableObject
{
    /// <summary>
    /// Fire a bullet pattern from the given position
    /// </summary>
    public abstract void Fire(Vector3 firePosition, float angle = 0f);

    /// <summary>
    /// Get a friendly name for this pattern
    /// </summary>
    public virtual string GetPatternName()
    {
        return this.name;
    }
}
