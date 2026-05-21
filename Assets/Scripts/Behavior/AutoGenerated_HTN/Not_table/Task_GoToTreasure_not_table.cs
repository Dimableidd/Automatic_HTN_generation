using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_GoToTreasure_not_table", menuName = "HTNTask/Task_GoToTreasure_not_table")]

public class Task_GoToTreasure_not_table : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "hasTreasure", false },
            { "enemyVisible", false },
            { "enemyInRange", false },
            { "treasureOnMap", true }
        };
    }

    public override Dictionary<string, object> Effects()
    {
        return new Dictionary<string, object>();
    }

    public override TaskResult Execute(Character character)
    {
        character.SetTarget(character.GetNearestTreasure()?.transform);
                if (character.Target == null)
                    return TaskResult.FAILURE;
                    
                character.Agent.SetDestination(character.Target.position);
                return TaskResult.PROCESSING;
    }
}