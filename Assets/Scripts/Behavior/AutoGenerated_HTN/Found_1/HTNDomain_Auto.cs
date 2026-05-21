using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HTNDomain_Auto", menuName = "HTNCompoundTask/HTNDomain_Auto")]

public class HTNDomain_Auto : HTNCompoundTask
{
    public override List<(Dictionary<string, object> PreConditions, List<HTNTask> Tasks)> GetMethods()
    {
        return new List<(Dictionary<string, object>, List<HTNTask>)>
        {
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", true },
                    { "enemyInRange", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_GoToBase>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", false },
                    { "enemyVisible", true },
                    { "enemyInRange", true },
                    { "treasureOnMap", false}
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_Attack>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", false },
                    { "enemyVisible", false },
                    { "enemyInRange", false },
                    { "treasureOnMap", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_Idle>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", false },
                    { "enemyInRange", false },
                    { "treasureOnMap", true }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_GoToTreasure>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", false },
                    { "enemyVisible", true },
                    { "enemyInRange", false },
                    { "treasureOnMap", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_GoToEnemy>()
                }
            )
        };
    }
}