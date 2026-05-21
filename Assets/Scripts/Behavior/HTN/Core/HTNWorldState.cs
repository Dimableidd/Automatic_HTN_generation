using UnityEngine;
using System;
using System.Collections.Generic;

public class HTNWorldState
{
    public event Action OnStateChanged;

    public Dictionary<string, object> state;

    public HTNWorldState()
    {
        state = new Dictionary<string, object>();
    }

    public void SetValue(string key, object value, bool notifyChanges = true)
    {
        if (state.ContainsKey(key) && state[key].Equals(value)) return;
        state[key] = value;
        if (notifyChanges)
            OnStateChanged?.Invoke();
    }

    public object GetValue(string key, object defaultValue = null)
    {
        return state.ContainsKey(key) ? state[key] : defaultValue;
    }

    public Dictionary<string, object> GetCurrentState()
    {
        return new Dictionary<string, object>(state);
    }

    public HTNWorldState Duplicate()
    {
        return new HTNWorldState { state = new Dictionary<string, object>(state) };
    }
    public void Get()
    {
        foreach (var key in state.Keys)
        {
            object value = state[key];
            string valueString;

            if (value is IFormattable)
            {
                valueString = ((IFormattable)value).ToString(null, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                valueString = value.ToString();
            }

            Debug.Log($"Key: {key}, Value: {valueString}");
        }
    }
}

