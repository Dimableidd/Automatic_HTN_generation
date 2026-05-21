using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_GoToBase_not_table", menuName = "HTNTask/Task_GoToBase_not_table")]

public class Task_GoToBase_not_table : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "hasTreasure", true },
            { "enemyVisible", false },
            { "enemyInRange", false }
        };
    }

    public override Dictionary<string, object> Effects()
    {
        return new Dictionary<string, object>
        {
            { "hasTreasure", false },
            { "treasureOnMap", false }
        };
    }

    public override TaskResult Execute(Character character)
    {
        character.Agent.SetDestination(character.HomeBase.position);
                return TaskResult.PROCESSING;
    }
}