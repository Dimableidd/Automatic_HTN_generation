using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_GoToTreasure", menuName = "HTNTask/Task_GoToTreasure_2")]

public class Task_GoToTreasure_2 : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "hasTreasure", false },
            { "enemyInRange", false },
            { "treasureOnMap", true },
            { "weaponPointOnMap", true },
            { "lowWeaponStrength", false },
            { "lowHP", false }
        };
    }

    public override Dictionary<string, object> Effects()
    {
        return new Dictionary<string, object>
        {
            { "enemyVisible", true }
        };
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