using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_Attack_not_table", menuName = "HTNTask/Task_Attack_not_table")]

public class Task_Attack_not_table : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "enemyVisible", true }
        };
    }

    public override Dictionary<string, object> Effects()
    {
        return new Dictionary<string, object>
        {
            { "hasTreasure", false },
            { "enemyVisible", false },
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