using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_GoToEnemy_not_table", menuName = "HTNTask/Task_GoToEnemy_not_table")]

public class Task_GoToEnemy_not_table : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "hasTreasure", false },
            { "enemyVisible", true },
            { "enemyInRange", false }
        };
    }

    public override Dictionary<string, object> Effects()
    {
        return new Dictionary<string, object>();
    }

    public override TaskResult Execute(Character character)
    {
        character.SetTarget(character.GetNearestEnemy()?.transform);
                if (character.Target == null)
                    return TaskResult.FAILURE;
                    
                character.Agent.SetDestination(character.Target.position);
                return TaskResult.PROCESSING;
    }
}