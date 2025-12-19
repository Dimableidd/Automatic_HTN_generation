using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_Attack", menuName = "HTNTask/Task_Attack")]

public class Task_Attack : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "enemyVisible", true },
            { "enemyInRange", true }
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