using UnityEngine;
using System.Collections.Generic;

public class HTNTask : ScriptableObject
{
    public enum TaskResult 
    {
        SUCCESS,
        FAILURE,
        PROCESSING,
    }

    public void ApplyEffects(HTNWorldState worldState)
    {
        var effects = Effects();
        foreach (var effect in effects)
        {
            worldState.SetValue(effect.Key, effect.Value);
        }
    }

    public bool IsAvailable(HTNWorldState worldState)
    {
        Dictionary<string, object> conditions = PreConditions();
        foreach (var condition in conditions)
        {
            if (!worldState.GetValue(condition.Key).Equals(condition.Value))
            {
                return false;
            }
        }
        return true;
    }

    public virtual Dictionary<string, object> PreConditions()
    {
        return new Dictionary<string, object>();
    }

    public virtual Dictionary<string, object> Effects()
    {
        return new Dictionary<string, object>();
    }

    public virtual TaskResult Execute(Character character)
    {
        return TaskResult.SUCCESS;
    }
}