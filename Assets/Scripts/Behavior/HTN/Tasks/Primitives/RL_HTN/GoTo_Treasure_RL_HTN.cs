using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GoTo_Treasure_RL_HTN", menuName = "HTNTask/GoTo_Treasure_RL_HTN")]
public class GoTo_Treasure_RL_HTN : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "has_treasure", false },
        { "is_enemy_visible", false },
        { "is_in_attack_range", false },
        { "treasure_exists", true }
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        { "has_treasure", true }
    };

    public override TaskResult Execute(Character character)
    {
        character.SetTarget(character.GetNearestTreasure()?.transform);
        if (character.Target == null)
            return TaskResult.FAILURE;
            
        character.Agent.SetDestination(character.Target.position);
        return TaskResult.PROCESSING;
    }
}
