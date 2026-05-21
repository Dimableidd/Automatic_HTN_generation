using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HTNDomain_Auto_RLvsRL", menuName = "HTNCompoundTask/HTNDomain_Auto_RLvsRL")]

public class HTNDomain_Auto_RLvsRL : HTNCompoundTask
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
                    CreateInstance<Task_GoToBase_RLvsRL>()
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
                    CreateInstance<Task_Idle_RLvsRL>()
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
                    CreateInstance<Task_GoToTreasure_RLvsRL>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", false },
                    { "enemyVisible", true },
                    { "enemyInRange", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_GoToEnemy_RLvsRL>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", false },
                    { "enemyVisible", true },
                    { "enemyInRange", true },
                    { "treasureOnMap", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_Attack_RLvsRL>()
                }
            )
        };
    }
}