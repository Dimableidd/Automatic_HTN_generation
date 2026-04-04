using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_Idle", menuName = "HTNTask/Task_Idle_2")]

public class Task_Idle_2 : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "hasTreasure", false },
            { "enemyVisible", false },
            { "enemyInRange", false },
            { "treasureOnMap", false },
            { "weaponPointOnMap", true },
            { "HPPointOnMap", true },
            { "lowWeaponStrength", false },
            { "lowHP", false }
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