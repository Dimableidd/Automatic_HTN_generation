using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Team : MonoBehaviour
{
    public GameManager gameManager;
    public SpawnTrasures spawnTrasures;
    public int teamName;
    public GameObject character;
    public int score = 0;

    public void AddScore(int points)
    {
        if(teamName == 0)
        {
            gameManager.Score_team_1 += points;
            //gameManager.comand_1.text = $"{gameManager.Score_team_1}";
        }
        else
        {
            gameManager.Score_team_2 += points;
            //gameManager.comand_2.text = $"{gameManager.Score_team_2}";
        }
    }

    public void StartDestroyCharacter(Character character)
    {
        StartCoroutine(DestroyAndRespawnCharacter(character));
    }

    public IEnumerator DestroyAndRespawnCharacter(Character character)
    {
        yield return new WaitForSeconds(10f);

        character.currentHealth = character.maxHealth;
        character.transform.localPosition = character.spawnPosition;
        character.currentWeaponStrength = character.maxWeaponStrength;
        character.boolChest = false;
        character.boolCoin = false;
        character.GetComponent<NavMeshAgent>().speed = 3.5f;
        character.enemy.Clear();
        if (character.Treasure != null)
            Destroy(character.Treasure);
        character.Treasure = null;        
        character.Target = null;
        character.gameObject.SetActive(true);
        character.Agent.ResetPath();
    }
}
