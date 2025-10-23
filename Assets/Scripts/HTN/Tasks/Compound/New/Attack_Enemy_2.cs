using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Attack_Enemy_2", menuName = "HTNCompoundTask/Attack_Enemy_2")]
public class Attack_Enemy_2 : HTNCompoundTask
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
                {
                    CreateInstance<GoTo_Enemy_2>(),
                    CreateInstance<Attack_2>()
                }
            )
        };
    }
}
