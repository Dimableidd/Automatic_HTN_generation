using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task_GoToHPPOint", menuName = "HTNTask/Task_GoToHPPOint_2")]

public class Task_GoToHPPOint_2 : HTNTask
{
    public override Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>
        {
            { "HPPointOnMap", true },
            { "middleHP", true }
        };
    }

    public override Dictionary<string, object> Effects()
    {
        return new Dictionary<string, object>
        {
            { "enemyInRange", false },
            { "treasureOnMap", true }
        };
    }

    public override TaskResult Execute(Character character)
    {
        character.SetTarget(character.GetComponentInParent<Team>().house.HP?.transform);
        if (character.Target == null)
            return TaskResult.FAILURE;
            
        character.Agent.SetDestination(character.Target.position);
        return TaskResult.PROCESSING;
    }
}