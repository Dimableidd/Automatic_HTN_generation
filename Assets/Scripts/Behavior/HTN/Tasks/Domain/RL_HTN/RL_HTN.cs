using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RL_HTN", menuName = "HTNCompoundTask/RL_HTN")]
public class RL_HTN : HTNCompoundTask
{
    public override List<(Dictionary<string, object> PreConditions, List<HTNTask> Tasks)> GetMethods()
    {
        return new List<(Dictionary<string, object>, List<HTNTask>)>
        {
            (
                new Dictionary<string, object>
                {
                    { "has_treasure", true },
                    { "is_enemy_visible", true },
                    { "is_in_attack_range", true }
                },
                new List<HTNTask>
                { CreateInstance<Attack_RL_HTN>() }
            ),

            (
                new Dictionary<string, object>
                {
                    { "has_treasure", false },
                    { "is_enemy_visible", false },
                    { "is_in_attack_range", false },
                    { "treasure_exists", true }
                },
                new List<HTNTask>
                {
                    CreateInstance<GoTo_Treasure_RL_HTN>()
                }
            ),

            (
                new Dictionary<string, object>
                {
                    { "has_treasure", true },
                    { "is_in_attack_range", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<GoTo_House_RL_HTN>()
                }
            ),

            (
                new Dictionary<string, object>
                {
                    { "has_treasure", false },
                    { "is_enemy_visible", false },
                    { "is_in_attack_range", false },
                    { "treasure_exists", true }
                },
                new List<HTNTask>
                {
                    CreateInstance<Idle_RL_HTN>()
                }
            )
        };
    }
}
