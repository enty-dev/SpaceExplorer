using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Executes a sequence of EnemyActions on an enemy.
/// This allows you to define complex enemy behavior as a series of simple actions:
/// 
/// Example:
///   runner.AddAction(new MoveAction(position))
///          .AddAction(new ShootAction(pattern))
///          .AddAction(new WaitAction(2f))
///          .AddAction(new LeaveScreenAction())
///          .Run();
/// </summary>
public class EnemyScriptRunner : MonoBehaviour
{
    private Queue<EnemyAction> actionQueue = new Queue<EnemyAction>();
    private EnemyAction currentAction = null;
    private bool isRunning = false;

    /// <summary>
    /// Add an action to the execution queue
    /// </summary>
    public EnemyScriptRunner AddAction(EnemyAction action)
    {
        if (action != null)
        {
            actionQueue.Enqueue(action);
        }
        return this; // Allow chaining
    }

    /// <summary>
    /// Start executing the queued actions
    /// </summary>
    public void Run()
    {
        if (actionQueue.Count > 0)
        {
            isRunning = true;
            ExecuteNextAction();
        }
    }

    /// <summary>
    /// Stop execution and clear the queue
    /// </summary>
    public void Stop()
    {
        isRunning = false;
        if (currentAction != null)
        {
            currentAction.OnCancel();
            currentAction = null;
        }
        actionQueue.Clear();
    }

    private void ExecuteNextAction()
    {
        if (actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            currentAction.Initialize();
        }
        else
        {
            isRunning = false;
            OnSequenceComplete();
        }
    }

    private void Update()
    {
        if (!isRunning || currentAction == null)
            return;

        // Execute current action
        if (currentAction.Execute())
        {
            // Action is complete, move to next
            ExecuteNextAction();
        }
    }

    /// <summary>
    /// Called when all actions have been completed
    /// Override in subclass if needed
    /// </summary>
    protected virtual void OnSequenceComplete()
    {
        // Can be overridden by subclasses
    }
}
