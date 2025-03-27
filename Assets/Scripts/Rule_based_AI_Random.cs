using UnityEngine;
using System.Collections.Generic;

public class Rule_based_AI_Random : MonoBehaviour
{

    public Character character;
    public List<GameObject> treasures;
    public Transform House;

    public void Awake()
    {
        character = transform.GetComponent<Character>();
    }

    public void Start()
    {
        treasures = SpawnTrasures.Instance.GetTreasures();
    }

    public void Update()
    {
        if (character.enemy.Count != 0)
        {
            if(!character.enemy.Contains(character.Target.gameObject))
            {
                character.Target = character.enemy[0].transform;
                character.Agent.SetDestination(character.Target.position);
            }

            else
            {
                float distance = Vector3.Distance(gameObject.transform.position, character.Target.transform.position);
                if (distance <= character.attackDist)
                {
                    character.Agent.ResetPath();
                    if (character.CanAttack())
                    {   
                        character.Attack(character.Target.gameObject);
                    }
                }
                else 
                    character.Agent.SetDestination(character.Target.position);
            }
            
        }
        else
        {
            if (character.Treasure == null)
            {
                if(character.Target == null || !treasures.Contains(character.Target.gameObject))
                {
                    treasures = SpawnTrasures.Instance.GetTreasures();
                    if (treasures.Count != 0)
                    {
                        character.Target = treasures[Random.Range(0,treasures.Count)].transform;
                        character.Agent.SetDestination(character.Target.position);
                    }
                    else
                        character.Agent.ResetPath();
                }
            }

            else
            {
                if(character.Target != House)
                {
                    character.Target = House;
                    character.Agent.SetDestination(character.Target.position);
                }
            }
        }
    }
}
