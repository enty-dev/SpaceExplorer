using UnityEngine;

/// <summary>
/// Abstract base class for enemy actions.
/// Actions are individual commands that enemies can execute (move, shoot, wait, etc.)
/// </summary>
public abstract class EnemyAction : MonoBehaviour
{
    /// <summary>
    /// Initialize the action before execution
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Execute one frame of the action
    /// </summary>
    /// <returns>true if action is complete, false if still executing</returns>
    public abstract bool Execute();

    /// <summary>
    /// Called when the action is cancelled or interrupted
    /// </summary>
    public virtual void OnCancel() { }
}
