    using UnityEngine;
    using System.Collections.Generic;

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        //public List<Team> teams = new List<Team>();
        public List<GameObject> prefabsTeams = new List<GameObject>();
        public GameObject prefabTresures;
        public int targetScore = 1500;
        public int maxRounds = 11; // Максимальное количество раундов
        public int currentRound = 1;

        public int CountWinTeamOne = 0;
        public int CountWinTeamTwo = 0;


        // Контейнеры для команд и сокровищ
        public List<GameObject> instantiatedTeams = new List<GameObject>();
        public GameObject instantiatedTreasures;

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            StartGame();
        }

        void Update()
        {
            CheckEnd();
        }

    public void StartGame()
        {

            // Создание сокровищ
            if (prefabTresures != null)
            {
                instantiatedTreasures = Instantiate(prefabTresures, Vector3.zero, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Префаб сокровищ не установлен в GameManager.");
            }

            // Создание команд
            foreach (GameObject teamPrefab in prefabsTeams)
            {
                if (teamPrefab != null)
                {
                    // Создаем экземпляр команды
                    GameObject teamInstance = Instantiate(teamPrefab, Vector3.zero, Quaternion.identity);
                    
                    // Добавляем в список для дальнейшего управления
                    instantiatedTeams.Add(teamInstance);
                }
                else
                {
                    Debug.LogError("Один из префабов команд не установлен в GameManager.");
                }
            }
        }

        public void CheckEnd()
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
        }

        public void ResetGame()
        {
            // Сброс сокровищ
            Destroy(instantiatedTreasures);
            // Сброс персонажей
            foreach (GameObject team in instantiatedTeams)
            {
                Destroy(team);
            }

            currentRound++;

            instantiatedTeams.Clear();

            GameObject[] iconsArray = GameObject.FindGameObjectsWithTag("Icon");

            foreach(GameObject obj in iconsArray)
                Destroy(obj);

            StartGame();
        }

        public void EndGame()
        {
            // Сброс сокровищ
            Destroy(instantiatedTreasures);
            // Сброс персонажей
            foreach (GameObject team in instantiatedTeams)
            {
                Destroy(team);
            }

        }


    }
