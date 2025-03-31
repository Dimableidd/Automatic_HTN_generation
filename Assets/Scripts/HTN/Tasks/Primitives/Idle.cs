using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Idle", menuName = "HTNTask/Idle")]
public class Idle : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "is_enemy_collider", false }
    };
    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        { "is_enemy_collider", false }
    };
    public override TaskResult Execute(Character character)
    {
        character.Target = null;
        character.Agent.ResetPath();
        return TaskResult.PROCESSING;
    }
}
