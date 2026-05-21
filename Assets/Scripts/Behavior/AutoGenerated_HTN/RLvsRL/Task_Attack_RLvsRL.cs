using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_Attack_RLvsRL", menuName = "HTNTask/Task_Attack_RLvsRL")]

public class Task_Attack_RLvsRL : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "hasTreasure", false },
            { "enemyVisible", true },
            { "enemyInRange", true },
            { "treasureOnMap", false }
        };
    }

    public override Dictionary<string, object> Effects()
    {
        return new Dictionary<string, object>
        {
            { "enemyInRange", false }
        };
    }

    public override TaskResult Execute(Character character)
    {
        GameObject enemy = character.GetNearestEnemy();
                if (enemy != null && character.CanAttack())
                {
                    character.Attack(enemy);
                }
                return TaskResult.PROCESSING;
    }
}