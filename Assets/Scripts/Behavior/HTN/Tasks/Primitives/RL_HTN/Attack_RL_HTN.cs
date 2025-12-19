using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Attack_RL_HTN", menuName = "HTNTask/Attack_RL_HTN")]
public class Attack_RL_HTN : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "has_treasure", true },
        { "is_enemy_visible", true },
        { "is_in_attack_range", true }
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        { "is_in_attack_range", false },
        { "treasure_exists", true }
    };

    public override TaskResult Execute(Character character)
    {
        GameObject enemy = character.GetNearestEnemy();
        if (enemy != null && character.CanAttack())
        {
            character.Attack(enemy);
        }
        return TaskResult.PROCESSING;
    }
}
