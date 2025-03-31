using System;
using UnityEngine;

public class NPCSensors : HTNAgentSensors
{
    public override void Initialize(GameObject actor)
    {
        SetState("is_enemy_collider", false, false);
        SetState("is_enemy_distance_attack", false, false);
    }
    public void Update()
    {
        GetEnemy();
        GetDistanceEnemy();
    }
    
    public void GetEnemy()
    {
        if(character.enemy.Count != 0) 
            SetState("is_enemy_collider", true);
        else 
            SetState("is_enemy_collider", false);
    }
    public void GetDistanceEnemy()
    {
        if(character.Target == null) return;
        if(character.enemy.Contains(character.Target.gameObject))
        {
            float distance = Vector3.Distance(character.gameObject.transform.position, character.Target.transform.position);
            if (distance <= character.attackDist)
            {
                SetState("is_enemy_distance_attack", true);
            }
            else
                SetState("is_enemy_distance_attack", false);
        }
    }
}
