using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_Idle_not_table", menuName = "HTNTask/Task_Idle_not_table")]

public class Task_Idle_not_table : HTNTask
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
        return new Dictionary<string, object>();
    }

    public override TaskResult Execute(Character character)
    {
        character.Agent.ResetPath();
                return TaskResult.PROCESSING;
    }
}