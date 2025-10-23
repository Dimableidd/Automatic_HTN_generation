using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GoTo_House_2", menuName = "HTNTask/GoTo_House_2")]
public class GoTo_House_2 : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "has_treasure", true }
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        { "has_treasure", false },
    };

    public override TaskResult Execute(Character character)
    {
        float distance = Vector3.Distance(character.transform.position, character.HomeBase.position);
        /*if (distance <= 2f)
        {
            character.DeliverTreasure();
            return TaskResult.SUCCESS;
        }*/

        character.Agent.SetDestination(character.HomeBase.position);
        return TaskResult.PROCESSING;
    }
}
