using UnityEngine;
using System.Collections.Generic;

public class HTNCompoundTask
{
    public virtual List<(Dictionary<string, object> PreConditions, List<HTNTask> Tasks)> GetMethods()
    {
        return new List<(Dictionary<string, object>, List<HTNTask>)>();
    }
}