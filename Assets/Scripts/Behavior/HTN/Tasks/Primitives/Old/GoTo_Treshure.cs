using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GoTo_Treashure", menuName = "HTNTask/GoTo_Treashure")]
public class GoTo_Treashure : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "is_a_treasures", true },
        { "is_a_treasures_on_character", false },
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        { "is_a_treasures", false },
        { "is_a_treasures_on_character", true },
    };

    public override TaskResult Execute(Character character)
    {
        if(character.Target != null || !character.Target.Equals(null) )
        {
            character.Agent.SetDestination(character.Target.position);
            return TaskResult.PROCESSING;
        }
        character.Agent.ResetPath();
        character.SetTarget(null);
        return TaskResult.FAILURE;

    }
}
