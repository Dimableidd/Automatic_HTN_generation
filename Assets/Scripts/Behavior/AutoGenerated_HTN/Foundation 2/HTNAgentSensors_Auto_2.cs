using System;
using UnityEngine;

public class HTNAgentSensors_Auto_2 : HTNAgentSensors
{

    public override void Initialize(GameObject actor)
    {
        SetState("hasTreasure", false, false);
        SetState("enemyVisible", false, false);
        SetState("enemyInRange", false, false);
        SetState("treasureOnMap", false, false);
        SetState("weaponPointOnMap", false, false);
        SetState("HPPointOnMap", false, false);
        SetState("highWeaponStrength", false, false);
        SetState("middleWeaponStrength", false, false);
        SetState("lowWeaponStrength", false, false);
        SetState("highHP", false, false);
        SetState("middleHP", false, false);
        SetState("lowHP", false, false);
    }

    private void Update()
    {
        SetState("hasTreasure", HasTreasure(), true);
        SetState("enemyVisible", IsEnemyVisible(), true);
        SetState("enemyInRange", IsEnemyInAttackRange(), true);
        SetState("treasureOnMap", HasTreasureOnMap(), true);
        SetState("weaponPointOnMap", HasWeaponPoint(), true);
        SetState("HPPointOnMap", HasHPPoint(), true);
        SetState("highWeaponStrength", HasHighWeaponStrength(), true);
        SetState("middleWeaponStrength", HasMiddleWeaponStrength(), true);
        SetState("lowWeaponStrength", HasLowWeaponStrength(), true);
        SetState("highHP", HasHighHP(), true);
        SetState("middleHP", HasMiddleHP(), true);
        SetState("lowHP", HasLowHP(), true);
    }

    private bool HasTreasure()
        {
            return character.boolChest || character.boolCoin;
        }

    private bool IsEnemyVisible()
        {
            bool visible = character.enemy != null && character.enemy.Count > 0;
            return visible;
        }

    private bool IsEnemyInAttackRange()
        {
            foreach (var enemy in character.enemy)
            {
                if (enemy == null) continue;
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= character.attackDist)
                    return true;
            }
            return false;
        }

    private bool HasTreasureOnMap()
        {
            return character.spawnTrasures.GetTreasures().Count > 0;
        }

    private bool HasWeaponPoint()
        {
            return GetComponentInParent<Team>().house.weapon.activeSelf;
        }

    private bool HasHPPoint()
        {
            return GetComponentInParent<Team>().house.HP.activeSelf;
        }

    private bool HasHighWeaponStrength()
        {
            return character.currentWeaponStrength >= 11 && character.currentWeaponStrength < 16;
        }

    private bool HasMiddleWeaponStrength()
        {
            return character.currentWeaponStrength >= 6 && character.currentWeaponStrength < 11;
        }

    private bool HasLowWeaponStrength()
        {
            return character.currentWeaponStrength >= 1 && character.currentWeaponStrength < 6;
        }

    private bool HasHighHP()
        {
            return character.currentHealth >= 15 && character.currentHealth < 21;
        }

    private bool HasMiddleHP()
        {
            return character.currentHealth >= 6 && character.currentHealth < 16;
        }

    private bool HasLowHP()
        {
            return character.currentHealth >= 1 && character.currentHealth < 6;
        }

}