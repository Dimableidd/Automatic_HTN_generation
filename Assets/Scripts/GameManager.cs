    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.UI;
using UnityEditor;

public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        //public List<Team> teams = new List<Team>();
        public List<GameObject> prefabsTeams = new List<GameObject>();
        public GameObject prefabTresures;
        public bool learning = false;
        public int targetScore = 1500;
        public int Score_team_1 = 0;
        public int Score_team_2 = 0;
        //public int maxRounds = 11; // Максимальное количество раундов
        public int currentRound = 1;

        public int CountWinTeamOne = 0;
        public int CountWinTeamTwo = 0;

        //public Text comand_1;
        //public Text comand_2;


        // Контейнеры для команд и сокровищ
        public List<GameObject> instantiatedTeams = new List<GameObject>();
        public GameObject instantiatedTreasures;

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            
            if (prefabTresures != null)
            {
                instantiatedTreasures = Instantiate(prefabTresures, Vector3.zero, Quaternion.identity);
            }

            foreach (GameObject teamPrefab in prefabsTeams)
            {
                if (teamPrefab != null)
                {
                    GameObject teamInstance = Instantiate(teamPrefab, Vector3.zero, Quaternion.identity);

                    instantiatedTeams.Add(teamInstance);
                }
            }
            
        }

        void Update()
        {
            //CheckEnd();
            CheckEndEpisode();
        }
        
        public void CheckEndEpisode()
        {
            if (Score_team_1 >= targetScore)
            {
                CountWinTeamOne += 1;
                ResetGame(true);
            }
            else if (Score_team_2 >= targetScore)
            {
                CountWinTeamTwo += 1;
                ResetGame(false);
            }
        }


    /*public void CheckEnd()
    {
        // Логика для завершения, если достигнуто максимальное количество раундов
        if (currentRound >= maxRounds && instantiatedTeams != null && instantiatedTreasures)
        {
            // Определение победителя по количеству побед
            Team winner = null;
            int maxWins = 0;

            if (CountWinTeamOne > CountWinTeamTwo)
            {
                winner = instantiatedTeams[0].GetComponent<Team>();
                maxWins = CountWinTeamOne;
            }
            else
            {
                winner = instantiatedTeams[1].GetComponent<Team>();
                maxWins = CountWinTeamTwo;
            }

            if (winner != null)
            {
                Debug.Log($"Winner of the game is {winner.teamName} with {maxWins} wins!");
                // Здесь можно добавить логику для завершения игры
            }

            EndGame();
        }
    }*/

        public void ResetGame(bool winTeam_1)
        {
            Destroy(instantiatedTreasures);

            if (prefabTresures != null)
            {
                instantiatedTreasures = Instantiate(prefabTresures, Vector3.zero, Quaternion.identity);
            }
            
            foreach(GameObject team in instantiatedTeams)
            {
                foreach(Transform child in team.transform)
                {
                    if(child.CompareTag("Player"))
                    {
                        if (child.GetComponent<RL_Agent>())
                        {
                            RL_Agent agent = child.GetComponent<RL_Agent>();
                            if (winTeam_1)
                                agent.AddReward(agent.winReward);
                            else
                                agent.AddReward(agent.loserReward);   
                            agent.EndEpisode();
                        }
                        else
                        {
                            Team teamObj = team.GetComponent<Team>();
                            Character character = child.GetComponent<Character>();
                            child.gameObject.SetActive(true);
                            character.currentHealth = character.maxHealth;
                            character.transform.position = character.spawnPosition;
                            character.Agent.ResetPath();
                            character.boolChest = false;
                            character.boolCoin = false;
                            character.enemy.Clear();
                            if (character.Treasure != null)
                                Destroy(character.Treasure);
                            character.Treasure = null;        
                            character.Target = null;
                        }
                    }
                }
            }

            currentRound++;

            Score_team_1 = 0;
            Score_team_2 = 0;
            //comand_1.text = $"{0}";
            //comand_2.text = $"{0}";
        }

        /*public void EndGame()
        {
            // Сброс сокровищ
            Destroy(instantiatedTreasures);
            // Сброс персонажей
            foreach (GameObject team in instantiatedTeams)
            {
                Destroy(team);
            }

        }*/


    }
