using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Team : MonoBehaviour
{
    public int teamName;
    public GameObject character;
    public int score = 0;

    public void AddScore(int points)
    {
        if(teamName == 0)
        {
            GameManager.Instance.Score_team_1 += points;
            //GameManager.Instance.comand_1.text = $"{GameManager.Instance.Score_team_1}";
        }
        else
        {
            GameManager.Instance.Score_team_2 += points;
            //GameManager.Instance.comand_2.text = $"{GameManager.Instance.Score_team_2}";
        }
    }

    public void StartDestroyCharacter(Character character)
    {
        StartCoroutine(DestroyAndRespawnCharacter(character));
    }

    public IEnumerator DestroyAndRespawnCharacter(Character character)
    {
        yield return new WaitForSeconds(5f);

        character.currentHealth = character.maxHealth;
        character.transform.position = character.spawnPosition;
        character.boolChest = false;
        character.boolCoin = false;
        character.enemy.Clear();
        if (character.Treasure != null)
            Destroy(character.Treasure);
        character.Treasure = null;        
        character.Target = null;
        character.gameObject.SetActive(true);
        character.Agent.ResetPath();
    }
}
