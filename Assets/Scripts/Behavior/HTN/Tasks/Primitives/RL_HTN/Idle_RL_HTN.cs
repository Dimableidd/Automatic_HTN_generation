using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Idle_RL_HTN", menuName = "HTNTask/Idle_RL_HTN")]
public class Idle_RL_HTN : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "has_treasure", false },
        { "is_enemy_visible", false },
        { "is_in_attack_range", false },
        { "treasure_exists", true }
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>();

    public override TaskResult Execute(Character character)
    {
        character.Agent.ResetPath();
        return TaskResult.PROCESSING;
    }
}
