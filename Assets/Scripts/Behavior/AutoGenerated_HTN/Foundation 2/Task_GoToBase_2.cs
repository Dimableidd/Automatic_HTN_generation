using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_GoToBase", menuName = "HTNTask/Task_GoToBase_2")]

public class Task_GoToBase_2 : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "hasTreasure", true },
            { "enemyInRange", false },
            { "weaponPointOnMap", true },
            { "HPPointOnMap", true },
            { "lowWeaponStrength", false },
            { "lowHP", false }
        };
    }

    public override Dictionary<string, object> Effects()
    {
        return new Dictionary<string, object>
        {
            { "enemyVisible", true },
            { "treasureOnMap", true },
            { "highHP", false },
            { "middleHP", true }
        };
    }

    public override TaskResult Execute(Character character)
    {
        character.Agent.SetDestination(character.HomeBase.position);
            return TaskResult.PROCESSING;
    }
}