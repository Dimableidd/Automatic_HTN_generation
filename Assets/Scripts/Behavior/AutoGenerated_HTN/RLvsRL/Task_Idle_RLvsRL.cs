using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_Idle_RLvsRL", menuName = "HTNTask/Task_Idle_RLvsRL")]

public class Task_Idle_RLvsRL : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "hasTreasure", false },
            { "enemyVisible", false },
            { "enemyInRange", false },
            { "treasureOnMap", false }
        };
    }

    public override Dictionary<string, object> Effects()
    {
        return new Dictionary<string, object>
        {
            { "treasureOnMap", true }
        };
    }

    public override TaskResult Execute(Character character)
    {
        character.Agent.ResetPath();
                return TaskResult.PROCESSING;
    }
}