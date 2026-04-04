using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_Attack", menuName = "HTNTask/Task_Attack_2")]

public class Task_Attack_2 : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "enemyVisible", true },
            { "enemyInRange", true },
            { "weaponPointOnMap", true },
            { "lowWeaponStrength", false }
        };
    }

    public override Dictionary<string, object> Effects()
    {
        return new Dictionary<string, object>
        {
            { "highWeaponStrength", true },
            { "middleWeaponStrength", false }
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