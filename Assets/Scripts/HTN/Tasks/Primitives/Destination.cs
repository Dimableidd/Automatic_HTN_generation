using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Destination", menuName = "HTNTask/Destination")]
public class Destination : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "is_enemy_collider", true }
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        { "is_enemy_distance_attack", true }
    };

    public override TaskResult Execute(Character character)
    {
        if(character.Target == null)
        {
            character.Target = character.enemy[0].transform;
            character.Agent.SetDestination(character.Target.position);
        }
        else
        {
            float distance = Vector3.Distance(character.gameObject.transform.position, character.Target.transform.position);
            if (distance <= character.attackDist)
            {
                character.Agent.ResetPath();
                return TaskResult.SUCCESS;
            }
            else 
            {
                character.Agent.SetDestination(character.Target.position);
                return TaskResult.PROCESSING;
            }
        }
        return TaskResult.FAILURE;
    }
}
