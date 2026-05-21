using System.IO;
using System.Text;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public int Count_Platform = 5;

    public int Global_Win_team_1 = 0;
    public int Global_Win_team_2 = 0;

    private int globalEpisodeCounter = 0;

    private string episodePath;
    private string winRatePath;

    private object locker = new object();

    private string rlLogPath;
    private string htnLogPath;

    private object logLock = new object();

    void Awake()
    {
        Instance = this;

        string folder = Path.Combine(Application.dataPath, "Metrics");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        episodePath = Path.Combine(folder, "episodes.csv");
        winRatePath = Path.Combine(folder, "winrate.csv");

        if (!File.Exists(episodePath))
        {
            File.WriteAllText(episodePath,
                "episode;score_team_1;score_team_2;time;destroy_1;destroy_2;treasure_1;treasure_2\n");
        }

        if (!File.Exists(winRatePath))
        {
            File.WriteAllText(winRatePath,
                "total_win_1;total_win_2\n");
        }

        rlLogPath = Path.Combine(folder, "rl_actions.csv");
        htnLogPath = Path.Combine(folder, "htn_actions.csv");

        if (!File.Exists(rlLogPath))
        {
            File.WriteAllText(rlLogPath,
                "agent_id;hasTreasure;enemyVisible;enemyInRange;treasureOnMap;action\n");
        }

        if (!File.Exists(htnLogPath))
        {
            File.WriteAllText(htnLogPath,
                "agent_id;hasTreasure;enemyVisible;enemyInRange;treasureOnMap;action\n");
        }
    }
    public void SetWinRate(int team_1, int team_2)
    {
        Global_Win_team_1 += team_1; 
        Global_Win_team_2 += team_2; 
    }

    public void SetMetrics(int score1, int score2, float time, int destr1, int destr2, int treas1, int treas2)
    {
        lock (locker)
        {
            globalEpisodeCounter++;

            string line =
                $"{globalEpisodeCounter};{score1};{score2};{time};{destr1};{destr2};{treas1};{treas2}\n";

            File.AppendAllText(episodePath, line);
        }
    }

    public void LogAction(bool isRL, int agentId, int hasTreasure, int enemyVisible, int enemyInRange, int treasureOnMap, int action)
    {
        string path = isRL ? rlLogPath : htnLogPath;

        string line =
            $"{agentId};{hasTreasure};{enemyVisible};{enemyInRange};{treasureOnMap};{action}\n";

        lock (logLock)
        {
            File.AppendAllText(path, line);
        }
    }

    public void EndSimulation()
    {
        Count_Platform--;

        if (Count_Platform == 0)
        {
            SaveWinRate();
            Debug.Log("Simulation finished. Data saved.");
            Application.Quit();
        }
    }

    private void SaveWinRate()
    {
        string line = $"{Global_Win_team_1};{Global_Win_team_2}\n";
        File.AppendAllText(winRatePath, line);
    }
}
