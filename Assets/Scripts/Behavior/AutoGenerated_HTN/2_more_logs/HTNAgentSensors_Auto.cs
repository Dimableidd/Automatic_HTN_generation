using System;
using UnityEngine;

public class HTNAgentSensors_Auto : HTNAgentSensors
{

    public override void Initialize(GameObject actor)
    {
        SetState("hasTreasure", false, false);
        SetState("enemyVisible", false, false);
        SetState("enemyInRange", false, false);
        SetState("treasureOnMap", false, false);
    }

    private void Update()
    {
        SetState("hasTreasure", HasTreasure(), true);
        SetState("enemyVisible", IsEnemyVisible(), true);
        SetState("enemyInRange", IsEnemyInAttackRange(), true);
        SetState("treasureOnMap", HasTreasureOnMap(), true);
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

}