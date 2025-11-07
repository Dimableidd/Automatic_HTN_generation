using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Choice", menuName = "HTNTask/Choice")]
public class Choice : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>
    {
        { "is_a_treasures", true },
        { "is_a_treasures_on_character", false },
    };

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>
    {
        
    };

    public override TaskResult Execute(Character character)
    {
        List<GameObject> treasures = character.spawnTrasures.GetTreasures();
        character.SetTarget(treasures[Random.Range(0,treasures.Count)].transform);
        return TaskResult.SUCCESS;
    }
}
