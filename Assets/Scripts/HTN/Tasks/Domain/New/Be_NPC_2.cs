using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Be_NPC_2", menuName = "HTNCompoundTask/Be_NPC_2")]
public class Be_NPC_2 : HTNCompoundTask
{
public override List<(Dictionary<string, object> PreConditions, List<HTNTask> Tasks)> GetMethods()
    {
        return new List<(Dictionary<string, object>, List<HTNTask>)>
        {
            (
                new Dictionary<string, object>
                {
                    { "is_enemy_visible", true }
                },
                new List<HTNTask>
                { CreateInstance<Attack_Enemy_2>() }
            ),

            (
                new Dictionary<string, object>
                {
                    { "has_treasure", false },
                    { "treasure_exists", true }
                },
                new List<HTNTask>
                {
                    CreateInstance<GoTo_Treasure_2>()
                }
            ),

            (
                new Dictionary<string, object>
                {
                    { "has_treasure", true }
                },
                new List<HTNTask>
                {
                    CreateInstance<GoTo_House_2>()
                }
            ),

            (
                new Dictionary<string, object>(),
                new List<HTNTask>
                {
                    CreateInstance<Idle_2>()
                }
            )
        };
    }
}
