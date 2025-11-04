using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Set_House", menuName = "HTNTask/Set_House")]
public class Set_House : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "is_a_treasures_on_character", true },
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        
    };

    public override TaskResult Execute(Character character)
    {
        character.SetTarget(character.transform.parent.GetComponentInChildren<House>().transform);
        return TaskResult.SUCCESS;
    }
}
