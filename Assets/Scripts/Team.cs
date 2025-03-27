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
        score += points;
    }

    public void Reset()
    {
        score = 0;
    }

    void Update()
    {

        if (score >= GameManager.Instance.targetScore)
        {
            if(teamName == 0)
            {
                GameManager.Instance.CountWinTeamOne += 1;
                Reset();
                GameManager.Instance.ResetGame();
                return;
            }
            else if(teamName == 1)
            {
                GameManager.Instance.CountWinTeamTwo += 1;
                Reset();
                GameManager.Instance.ResetGame();
                return;
            }

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
