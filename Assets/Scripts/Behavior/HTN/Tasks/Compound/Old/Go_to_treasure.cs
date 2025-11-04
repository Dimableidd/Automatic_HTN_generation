using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Go_to_treasure", menuName = "HTNCompoundTask/Go_to_treasure")]
public class Go_to_treasure : HTNCompoundTask
{
public override List<(Dictionary<string, object> PreConditions, List<HTNTask> Tasks)> GetMethods()
    {
        return new List<(Dictionary<string, object>, List<HTNTask>)>
        {
            (
                new Dictionary<string, object>
                {
                    { "is_a_treasures", true },
                    { "is_a_treasures_on_character", false },
                },
                new List<HTNTask>
                {   CreateInstance<Choice>(),
                    CreateInstance<GoTo_Treashure>()
                }
            )
        };
    }
}
