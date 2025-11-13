using System.IO;
using System.Text;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RL_Agent : Agent
{

    [Header("Links")]
    public Character character;
    public Transform homeBase;
    public Team team;
    private Vector3 spawnPosition;


    [Header("Rewards (для логов)")]
    public float stepPenalty = -0.001f;
    public float attackReward = 0.05f;
    public float treasureRewardUp = 1f;
    public float treasureRewardDown = 2f;
    public float winReward = 5.0f;
    public float loserReward = -5.0f;
    public float deathPenalty = -1.0f;

    [Header("Logging")]
    [Tooltip("Включить логирование (автоматически включается при инференсе)")]
    public bool forceLogging = false;

    private bool loggingEnabled = false;
    private string logPath;
    private StringBuilder logBuffer = new StringBuilder();
    private int stepCount = 0;
    private int episodeCount = 0;

    public override void Initialize()
    {
        character = GetComponent<Character>();
        team = GetComponentInParent<Team>();
        homeBase = character.HomeBase;
        spawnPosition = transform.position;

        bool trainingMode = Academy.Instance.IsCommunicatorOn;
        loggingEnabled = !trainingMode || forceLogging;

        if (loggingEnabled)
        {
            string logsFolder = Path.Combine(Application.dataPath, "Logs");
            if (!Directory.Exists(logsFolder))
                Directory.CreateDirectory(logsFolder);

            string agentId = GetInstanceID().ToString();
            logPath = Path.Combine(logsFolder, $"Agent_{agentId}.csv");

            File.WriteAllText(logPath,
                "agent_id;episode;step;action;reward;hasTreasure;enemyVisible;enemyInRange;treasureOnMap;distTreasure;distEnemy;distBase;health\n");

            Debug.Log($"<color=cyan>[RL_Agent]</color> Logging ENABLED for Agent {agentId}");
        }
        else
        {
            Debug.Log("<color=yellow>[RL_Agent]</color> Training mode detected — logging disabled");
        }
    }

    public override void OnEpisodeBegin()
    {
        episodeCount++;
        stepCount = 0;

        Team teamObj = team.GetComponent<Team>();
        character.currentHealth = character.maxHealth;
        transform.localPosition = character.spawnPosition;
        character.Agent.ResetPath();
        character.boolChest = false;
        character.boolCoin = false;
        character.enemy.Clear();
        if (character.Treasure != null)
            Destroy(character.Treasure);
        character.Treasure = null;        
        character.Target = null;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(HasTreasure() ? 1f : 0f);
        sensor.AddObservation(IsEnemyVisible() ? 1f : 0f);
        sensor.AddObservation(IsEnemyInAttackRange() ? 1f : 0f);
        sensor.AddObservation(HasTreasureOnMap() ? 1f : 0f);

        sensor.AddObservation(GetNormalizedDistanceToTreasure());
        sensor.AddObservation(GetNormalizedDistanceToEnemy());
        sensor.AddObservation(GetNormalizedDistanceToBase());
        sensor.AddObservation(character.currentHealth / character.maxHealth);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];
        AddReward(stepPenalty);

        switch (action)
        {
            case 0: // Idle
                character.Agent.ResetPath();
                break;

            case 1: // Go to treasure
                MoveTo(GetNearestTreasure()?.transform);
                break;

            case 2: // Go to base
                MoveTo(homeBase);
                break;

            case 3: // Go to enemy
                MoveTo(GetNearestEnemy()?.transform);
                break;

            case 4: // Attack
                TryAttack();
                break;
        }

        if (loggingEnabled)
        {
            float reward = GetCumulativeReward();
            string agentId = GetInstanceID().ToString();

            var obs = new float[8];
            obs[0] = HasTreasure() ? 1f : 0f;
            obs[1] = IsEnemyVisible() ? 1f : 0f;
            obs[2] = IsEnemyInAttackRange() ? 1f : 0f;
            obs[3] = HasTreasureOnMap() ? 1f : 0f;
            obs[4] = GetNormalizedDistanceToTreasure();
            obs[5] = GetNormalizedDistanceToEnemy();
            obs[6] = GetNormalizedDistanceToBase();
            obs[7] = (float)character.currentHealth / (float)character.maxHealth;

            string line = $"{agentId};{episodeCount};{stepCount};{action};{reward};" +
                          $"{string.Join(";", obs)}\n";

            logBuffer.Append(line);

            if (stepCount % 200 == 0)
            {
                File.AppendAllText(logPath, logBuffer.ToString());
                logBuffer.Clear();
            }
        }

        stepCount++;
    }

    private void OnApplicationQuit()
    {
        if (loggingEnabled && logBuffer.Length > 0)
            File.AppendAllText(logPath, logBuffer.ToString());
    }

    public void AddRewardDeath()
    {
        AddReward(deathPenalty);
    }

    public void AddRewardUpTreashure()
    {
        AddReward(treasureRewardUp);
    }
    public void AddRewardDownTreashure()
    {
        AddReward(treasureRewardDown);
    }


    private bool HasTreasure()
    {
        return character.boolChest || character.boolCoin;
    }

    private bool HasTreasureOnMap()
    {
        return character.spawnTrasures.GetTreasures().Count > 0;
    }

    private bool IsEnemyVisible()
    {
        bool visible = character.enemy != null && character.enemy.Count > 0;
        return visible;
    }

    private bool IsEnemyInAttackRange()
    {
        foreach (var enemy in character.enemy)
        {
            if (enemy == null) continue;
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= character.attackDist)
                return true;
        }
        return false;
    }

    private float GetNormalizedDistanceToTreasure()
    {
        var treasure = GetNearestTreasure();
        if (treasure == null) return 1f;
        return Mathf.Clamp01(Vector3.Distance(transform.position, treasure.transform.position) / 100f);
    }

    private float GetNormalizedDistanceToEnemy()
    {
        var enemy = GetNearestEnemy();
        if (enemy == null) return 1f;
        return Mathf.Clamp01(Vector3.Distance(transform.position, enemy.transform.position) / 100f);
    }

    private float GetNormalizedDistanceToBase()
    {
        if (homeBase == null) return 1f;
        return Mathf.Clamp01(Vector3.Distance(transform.position, homeBase.position) / 100f);
    }



    private void MoveTo(Transform target)
    {
        if (target == null) return;
        character.Agent.SetDestination(target.position);
        character.Target = target;
    }

    private void TryAttack()
    {
        GameObject enemy = GetNearestEnemy();
        if (enemy != null && character.CanAttack())
        {
            character.Attack(enemy);
            AddReward(attackReward);
        }
    }


    private GameObject GetNearestTreasure()
    {
        var treasures = character.spawnTrasures.GetTreasures();
        float min = float.MaxValue;
        GameObject nearest = null;
        foreach (var t in treasures)
        {
            float d = Vector3.Distance(transform.position, t.transform.position);
            if (d < min) { min = d; nearest = t; }
        }
        return nearest;
    }

    private GameObject GetNearestEnemy()
    {
        float min = float.MaxValue;
        GameObject nearest = null;
        foreach (var e in character.enemy)
        {
            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < min) { min = d; nearest = e; }
        }
        return nearest;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Random.Range(0, 5);
    }

}
