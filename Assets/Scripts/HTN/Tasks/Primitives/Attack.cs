using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Attack", menuName = "HTNTask/Attack")]
public class Attack : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "is_enemy_distance_attack", true }
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        { "is_enemy_distance_attack", false }
    };

    public override TaskResult Execute(Character character)
    {
        if (character.CanAttack())
        {   
            character.Attack(character.Target.gameObject);
            Debug.Log("sss");
        }
        return TaskResult.PROCESSING;
    }
}
