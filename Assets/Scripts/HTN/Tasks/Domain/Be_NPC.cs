using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Be_NPC", menuName = "HTNCompoundTask/Be_NPC")]
public class Be_NPC : HTNCompoundTask
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
                { CreateInstance<Attack_Enemy>() }
            ),


            (
                new Dictionary<string, object>
                {
                    { "is_enemy_collider", false }
                },
                new List<HTNTask>
                { CreateInstance<Idle>() }
            ),
        };
    }
}
