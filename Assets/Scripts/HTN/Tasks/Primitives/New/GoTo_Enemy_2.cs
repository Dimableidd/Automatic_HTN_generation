using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GoTo_Enemy_2", menuName = "HTNTask/GoTo_Enemy_2")]
public class GoTo_Enemy_2 : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "is_enemy_visible", true }
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        { "is_in_attack_range", true }
    };

    public override TaskResult Execute(Character character)
    {
        if (character.Target == null || !character.enemy.Contains(character.Target.gameObject))
        {
            character.SetTarget(character.GetNearestEnemy()?.transform);
            if (character.Target == null)
                return TaskResult.FAILURE;
        }

        float distance = Vector3.Distance(character.transform.position, character.Target.position);
        if (distance <= character.attackDist)
        {
            character.Agent.ResetPath();
            return TaskResult.SUCCESS;
        }

        character.Agent.SetDestination(character.Target.position);
        return TaskResult.PROCESSING;
    }
}
