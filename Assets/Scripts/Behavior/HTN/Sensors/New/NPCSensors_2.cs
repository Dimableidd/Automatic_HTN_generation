using System;
using UnityEngine;

public class NPCSensors_2 : HTNAgentSensors
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
        if (character.Target == null || !character.enemy.Contains(character.Target.gameObject))
        {
            SetState("is_in_attack_range", false, false);
            return;
        }

        float distance = Vector3.Distance(character.transform.position, character.Target.transform.position);
        bool inRange = distance <= character.attackDist;

        SetState("is_in_attack_range", inRange, false);
    }

    private void UpdateTreasureAvailability()
    {
        bool exists = SpawnTrasures.Instance != null && SpawnTrasures.Instance.GetTreasures().Count > 0;
        SetState("treasure_exists", exists, true);
    }

    private void UpdateHasTreasure()
    {
        bool has = character.boolChest || character.boolCoin;
        SetState("has_treasure", has, true);
    }

}
