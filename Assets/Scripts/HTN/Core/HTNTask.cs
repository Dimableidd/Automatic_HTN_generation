using UnityEngine;
using System.Collections.Generic;

public class HTNTask
{
    public enum TaskResult 
    {
        SUCCESS,
        FAILURE,
        PROCESSING,
    }

    public void ApplyEffects(HTNWorldState worldState)
    {
        var effects = _Effects();
        foreach (var effect in effects)
        {
            worldState.SetValue(effect.Key, effect.Value);
        }
    }

    public bool IsAvailable(HTNWorldState worldState)
    {
        Dictionary<string, object> conditions = _PreConditions();
        foreach (var condition in conditions)
        {
            if (!worldState.GetValue(condition.Key).Equals(condition.Value))
            {
                return false;
            }
        }
        return true;
    }

    private Dictionary<string, object> _Effects()
    {
        return new Dictionary<string, object>();
    }

    private Dictionary<string, object> _PreConditions()
    {
        return new Dictionary<string, object>();
    }

    public virtual TaskResult Execute(GameObject actor)
    {
        return TaskResult.SUCCESS;
    }
}