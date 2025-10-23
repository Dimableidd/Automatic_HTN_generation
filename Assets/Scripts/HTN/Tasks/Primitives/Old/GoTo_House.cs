using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GoTo_House", menuName = "HTNTask/GoTo_House")]
public class GoTo_House : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "is_a_treasures_on_character", true },
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        { "is_a_treasures_on_character", false },
    };

    public override TaskResult Execute(Character character)
    {
        character.Agent.SetDestination(character.Target.position);
        return TaskResult.PROCESSING;
    }
}
