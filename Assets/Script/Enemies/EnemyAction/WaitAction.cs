using UnityEngine;

/// <summary>
/// Waits for a specified duration
/// </summary>
public class WaitAction : EnemyAction
{
    private float duration;
    private float elapsedTime;

    public WaitAction(float waitDuration)
    {
        duration = waitDuration;
    }

    public override void Initialize()
    {
        elapsedTime = 0f;
    }

    public override bool Execute()
    {
        elapsedTime += Time.deltaTime;
        return elapsedTime >= duration;
    }
}
