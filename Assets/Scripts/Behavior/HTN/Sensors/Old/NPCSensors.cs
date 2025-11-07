using System;
using UnityEngine;

public class NPCSensors : HTNAgentSensors
{
    public override void Initialize(GameObject actor)
    {
        SetState("target_character", false, false);

        SetState("is_enemy_collider", false, false);
        SetState("is_enemy_distance_attack", false, false);
        SetState("is_a_treasures", true, false);
        SetState("is_a_treasures_on_character", false, false);

        character.TargetAction += GetTarget;
    }
    public void Update()
    {
        //GetTarget();
        GetEnemy();
        GetDistanceEnemy();
        GetTreasures();
        GetTreasuresOnCharacter();
    }


    public void GetTarget()
    {
        if(character.Target == null || character.Target.Equals(null))
            SetState("target_character", false);
        else
            SetState("target_character", true, false);
    }
    public void GetEnemy()
    {
        if(character.enemy.Count != 0) 
            SetState("is_enemy_collider", true);
        else 
            SetState("is_enemy_collider", false);
    }
    public void GetTreasures()
    {
        if(character.spawnTrasures.GetTreasures().Count != 0) 
            SetState("is_a_treasures", true);
        else 
            SetState("is_a_treasures", false, false);
    }

    public void GetTreasuresOnCharacter()
    {
        if(character.Treasure != null) 
            SetState("is_a_treasures_on_character", true);
        else 
            SetState("is_a_treasures_on_character", false);
    }
    public void GetDistanceEnemy()
    {
        if(character.Target == null || character.Target.Equals(null)) return;
        if(character.enemy.Contains(character.Target.gameObject))
        {
            float distance = Vector3.Distance(character.gameObject.transform.position, character.Target.transform.position);
            if (distance <= character.attackDist)
            {
                SetState("is_enemy_distance_attack", true, false);
            }
            else
                SetState("is_enemy_distance_attack", false, false);
        }
    }
}
