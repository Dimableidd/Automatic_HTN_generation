using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Go_to_house", menuName = "HTNCompoundTask/Go_to_house")]
public class Go_to_house : HTNCompoundTask
{
public override List<(Dictionary<string, object> PreConditions, List<HTNTask> Tasks)> GetMethods()
    {
        return new List<(Dictionary<string, object>, List<HTNTask>)>
        {
            (
                new Dictionary<string, object>
                {
                    { "is_a_treasures_on_character", true }
                },
                new List<HTNTask>
                {   CreateInstance<Set_House>(),
                    CreateInstance<GoTo_House>()
                }
            )
        };
    }
}
