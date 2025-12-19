using System;
using UnityEngine;

public class NPCSensors_RL_HTN : HTNAgentSensors
{
    public override void Initialize(GameObject actor)
    {
        SetState("is_enemy_visible", false, false);
        SetState("is_in_attack_range", false, false);
        SetState("treasure_exists", false, false);
        SetState("has_treasure", false, false);

    }

    private void Update()
    {
        UpdateEnemyVisibility();
        UpdateEnemyRange();
        UpdateTreasureAvailability();
        UpdateHasTreasure();
    }

    private void UpdateEnemyVisibility()
    {
        bool visible = character.enemy != null && character.enemy.Count > 0;
        SetState("is_enemy_visible", visible, true);
    }

    private void UpdateEnemyRange()
    {
        foreach (var enemy in character.enemy)
        {
            if (enemy == null) continue;
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= character.attackDist)
            {
                SetState("is_in_attack_range", true, true);
                return;
            }
        }
        SetState("is_in_attack_range", false, true);
        return;
    }

    private void UpdateTreasureAvailability()
    {
        bool exists = character.spawnTrasures != null && character.spawnTrasures.GetTreasures().Count > 0;
        SetState("treasure_exists", exists, true);
    }

    private void UpdateHasTreasure()
    {
        bool has = character.boolChest || character.boolCoin;
        SetState("has_treasure", has, true);
    }

}
