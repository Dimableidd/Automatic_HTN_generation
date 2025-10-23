using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Attack_2", menuName = "HTNTask/Attack_2")]
public class Attack_2 : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "is_in_attack_range", true }
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        { "is_in_attack_range", false }
    };

    public override TaskResult Execute(Character character)
    {
        if (character.CanAttack())
        {
            character.Attack(character.Target.gameObject);
            //return TaskResult.SUCCESS;
        }
        return TaskResult.PROCESSING;
    }
}
