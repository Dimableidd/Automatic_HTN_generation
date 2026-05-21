using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_GoToBase_RLvsRL", menuName = "HTNTask/Task_GoToBase_RLvsRL")]

public class Task_GoToBase_RLvsRL : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "hasTreasure", true },
            { "enemyInRange", false }
        };
    }

    public override Dictionary<string, object> Effects()
    {
        return new Dictionary<string, object>();
    }

    public override TaskResult Execute(Character character)
    {
        character.Agent.SetDestination(character.HomeBase.position);
                return TaskResult.PROCESSING;
    }
}