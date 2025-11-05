using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RL_Agent : Agent
{

    [Header("Links")]
    public Character character => gameObject.GetComponent<Character>();
    public Transform homeBase;
    public Team team;

    public float stepPenalty = -0.001f;
    public float attackReward = 0.05f;
    public float treasureRewardUp = 1f;
    public float treasureRewardDown = 2f;
    public float winReward = 5.0f;
    public float loserReward = -5.0f;
    public float deathPenalty = -1.0f;

    public void Start()
    {
        homeBase = character.HomeBase;
        team = gameObject.GetComponentInParent<Team>();
    }

    public override void OnEpisodeBegin()
    {

        if (GameManager.Instance.Score_team_1 >= GameManager.Instance.targetScore || GameManager.Instance.Score_team_2 >= GameManager.Instance.targetScore)
        {
            Team teamObj = team.GetComponent<Team>();
            gameObject.SetActive(true);
            character.currentHealth = character.maxHealth;
            transform.position = character.spawnPosition;
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
        float reward = stepPenalty;

        switch (action)
        {
            case 0: // Idle
                character.Agent.ResetPath();
                break;

            case 1: // Go to treasure
                var treasure = GetNearestTreasure();
                if (treasure != null)
                    MoveTo(treasure.transform);
                break;

            case 2: // Go to base
                MoveTo(homeBase);
                break;

            case 3: // Go to enemy
                var enemy = GetNearestEnemy();
                if (enemy != null)
                    MoveTo(enemy.transform);
                break;

            case 4: // Attack
                TryAttack();
                break;
        }
        AddReward(reward);
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
        return SpawnTrasures.Instance.GetTreasures().Count > 0;
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
        var treasures = SpawnTrasures.Instance.GetTreasures();
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
