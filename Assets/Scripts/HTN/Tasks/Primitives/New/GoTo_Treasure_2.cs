using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GoTo_Treasure_2", menuName = "HTNTask/GoTo_Treasure_2")]
public class GoTo_Treasure_2 : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "treasure_exists", true },
        { "has_treasure", false }
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        { "has_treasure", true }
    };

    public override TaskResult Execute(Character character)
    {
        if (character.Target == null)
        {
            character.SetTarget(character.GetNearestTreasure()?.transform);
            if (character.Target == null)
                return TaskResult.FAILURE;
        }

        float distance = Vector3.Distance(character.transform.position, character.Target.position);
        /*if (distance <= 1.5f)
        {
            character.PickTreasure(character.Target.gameObject);
            return TaskResult.SUCCESS;
        }*/

        character.Agent.SetDestination(character.Target.position);
        return TaskResult.PROCESSING;
    }
}
