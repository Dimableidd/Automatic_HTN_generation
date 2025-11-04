using UnityEngine;
using System.Collections.Generic;

public class Rule_based_AI_Nearest : MonoBehaviour
{
    public Character character;
    public List<GameObject> treasures;
    public Transform House;

    public void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Start()
    {
        treasures = SpawnTrasures.Instance.GetTreasures();
    }

    public void Update()
    {
        if (character.enemy.Count != 0)
        {
            HandleEnemyLogic();
        }
        else
        {
            HandleTreasureLogic();
        }
    }

    private void HandleEnemyLogic()
    {
        if (character.Target == null || !character.enemy.Contains(character.Target.gameObject))
        {
            character.Target = character.enemy[0].transform;
            character.Agent.SetDestination(character.Target.position);
        }
        else
        {
            float distance = Vector3.Distance(transform.position, character.Target.position);
            if (distance <= character.attackDist)
            {
                character.Agent.ResetPath();
                if (character.CanAttack())
                    character.Attack(character.Target.gameObject);
            }
            else
            {
                character.Agent.SetDestination(character.Target.position);
            }
        }
    }

    private void HandleTreasureLogic()
    {
        if (character.Treasure == null)
        {
            if (character.Target == null || !treasures.Contains(character.Target.gameObject))
            {
                treasures = SpawnTrasures.Instance.GetTreasures();
                if (treasures.Count > 0)
                {
                    GameObject nearest = GetNearestTreasure();
                    if (nearest != null)
                    {
                        character.Target = nearest.transform;
                        character.Agent.SetDestination(nearest.transform.position);
                    }
                }
                else
                {
                    character.Agent.ResetPath();
                }
            }
        }
        else
        {
            if (character.Target != House)
            {
                character.Target = House;
                character.Agent.SetDestination(House.position);
            }
        }
    }

    private GameObject GetNearestTreasure()
    {
        GameObject nearest = null;
        float minDistance = float.MaxValue;

        foreach (var t in treasures)
        {
            if (t == null) continue;
            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = t;
            }
        }
        return nearest;
    }
}
