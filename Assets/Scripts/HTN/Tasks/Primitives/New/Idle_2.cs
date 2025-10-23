using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Idle_2", menuName = "HTNTask/Idle_2")]
public class Idle_2 : HTNTask
{
    public override Dictionary<string, object> PreConditions() => new Dictionary<string, object>();

    public override Dictionary<string, object> Effects() => new Dictionary<string, object>();

    public override TaskResult Execute(Character character)
    {
        character.Agent.ResetPath();
        return TaskResult.PROCESSING;
    }
}
