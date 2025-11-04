using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Attack_Enemy", menuName = "HTNCompoundTask/Attack_Enemy")]
public class Attack_Enemy : HTNCompoundTask
{
public override List<(Dictionary<string, object> PreConditions, List<HTNTask> Tasks)> GetMethods()
    {
        return new List<(Dictionary<string, object>, List<HTNTask>)>
        {
            (
                new Dictionary<string, object>
                {
                    { "is_enemy_collider", true }
                },
                new List<HTNTask>
                {   CreateInstance<Set_target_enemy>(),
                    CreateInstance<Destination>(),
                    CreateInstance<Attack>() }
            )
        };
    }
}
