using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_GoToWeaponPoint", menuName = "HTNTask/Task_GoToWeaponPoint_2")]

public class Task_GoToWeaponPoint_2 : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "hasTreasure", false },
            { "treasureOnMap", false },
            { "weaponPointOnMap", true },
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
        character.SetTarget(character.GetComponentInParent<Team>().house.weapon?.transform);
        if (character.Target == null)
            return TaskResult.FAILURE;
            
        character.Agent.SetDestination(character.Target.position);
        return TaskResult.PROCESSING;
    }
}