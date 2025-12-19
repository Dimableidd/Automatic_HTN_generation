using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GoTo_House_RL_HTN", menuName = "HTNTask/GoTo_House_RL_HTN")]
public class GoTo_House_RL_HTN : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "has_treasure", true },
        { "is_in_attack_range", false }
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>();

    public override TaskResult Execute(Character character)
    {
        character.Agent.SetDestination(character.HomeBase.position);
        return TaskResult.PROCESSING;
    }
}
