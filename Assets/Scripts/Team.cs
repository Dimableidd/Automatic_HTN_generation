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
            GameManager.Instance.comand_1.text = $"{GameManager.Instance.Score_team_1}";
        }
        else
        {
            GameManager.Instance.Score_team_2 += points;
            GameManager.Instance.comand_2.text = $"{GameManager.Instance.Score_team_2}";
        }
    }

    public void StartDestroyCharacter()
    {
        StartCoroutine(DestroyAndRespawnCharacter());
    }

    public IEnumerator DestroyAndRespawnCharacter()
    {

        yield return new WaitForSeconds(5f); // Ждем 5 секунд


        if (character != null)
        {
            Vector3 spawnPosition = new Vector3();
            if (teamName == 0)
                spawnPosition = new Vector3(0f, 1f, -40f);
            else if (teamName == 1)
                spawnPosition = new Vector3(0f, 1f, 40f);
            
            Instantiate(character, spawnPosition, Quaternion.identity, transform);
        }
    }
}
