using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HTNDomain_Auto", menuName = "HTNCompoundTask/HTNDomain_Auto_2")]

public class HTNDomain_Auto_2 : HTNCompoundTask
{
    public override List<(Dictionary<string, object> PreConditions, List<HTNTask> Tasks)> GetMethods()
    {
        return new List<(Dictionary<string, object>, List<HTNTask>)>
        {
            (
                new Dictionary<string, object>
                {
                    { "weaponPointOnMap", true },
                    { "HPPointOnMap", true },
                    { "lowWeaponStrength", false },
                    { "highHP", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_GoToHPPOint_2>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", false },
                    { "enemyVisible", true },
                    { "enemyInRange", false },
                    { "weaponPointOnMap", true },
                    { "lowWeaponStrength", false },
                    { "lowHP", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_GoToEnemy_2>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", true },
                    { "enemyInRange", false },
                    { "weaponPointOnMap", true },
                    { "HPPointOnMap", true },
                    { "lowWeaponStrength", false },
                    { "lowHP", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_GoToBase_2>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", false },
                    { "enemyInRange", false },
                    { "treasureOnMap", true },
                    { "weaponPointOnMap", true },
                    { "lowWeaponStrength", false },
                    { "lowHP", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_GoToTreasure_2>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", false },
                    { "enemyVisible", true },
                    { "enemyInRange", true },
                    { "treasureOnMap", false },
                    { "weaponPointOnMap", true },
                    { "lowWeaponStrength", false },
                    { "highHP", true },
                    { "middleHP", false },
                    { "lowHP", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_Attack_2>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", false },
                    { "enemyVisible", false },
                    { "enemyInRange", false },
                    { "treasureOnMap", false },
                    { "weaponPointOnMap", true },
                    { "HPPointOnMap", true },
                    { "lowWeaponStrength", false },
                    { "lowHP", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_Idle_2>()
                }
            ),
            (
                new Dictionary<string, object>
                {
                    { "hasTreasure", false },
                    { "treasureOnMap", false },
                    { "weaponPointOnMap", true },
                    { "lowWeaponStrength", false },
                    { "lowHP", false }
                },
                new List<HTNTask>
                {
                    CreateInstance<Task_GoToWeaponPoint_2>()
                }
            )
        };
    }
}