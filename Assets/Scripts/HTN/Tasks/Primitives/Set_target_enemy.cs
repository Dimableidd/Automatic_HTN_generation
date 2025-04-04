using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Set_target_enemy", menuName = "HTNTask/Set_target_enemy")]
public class Set_target_enemy : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "is_enemy_collider", true }
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {

    };

    public override TaskResult Execute(Character character)
    {
        character.SetTarget(character.enemy[0].transform);
        return TaskResult.SUCCESS;
    }
}
