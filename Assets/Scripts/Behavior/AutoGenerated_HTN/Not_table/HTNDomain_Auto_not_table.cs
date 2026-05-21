using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HTNDomain_Auto_not_table", menuName = "HTNCompoundTask/HTNDomain_Auto_not_table")]

public class HTNDomain_Auto_not_table : HTNCompoundTask
{
    public override List<(Dictionary<string, object> PreConditions, List<HTNTask> Tasks)> GetMethods()
    {
        return new List<(Dictionary<string, object>, List<HTNTask>)>
        {
            (
                new Dictionary<string, object>
                {
                    { "enemyVisible", true }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_Attack_not_table>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", true },
                    { "enemyVisible", false },
                    { "enemyInRange", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_GoToBase_not_table>()
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
                    CreateInstance<Task_GoToEnemy_not_table>()
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
                    CreateInstance<Task_Idle_not_table>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", false },
                    { "enemyVisible", false },
                    { "enemyInRange", false },
                    { "treasureOnMap", true }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_GoToTreasure_not_table>()
                }
            )
        };
    }
}