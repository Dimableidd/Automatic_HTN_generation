using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnTrasures : MonoBehaviour
{
    public static SpawnTrasures Instance;

    [Header("Prefabs")]
    public GameObject chestPrefab;
    public GameObject coinPrefab;

    [Header("Spawn Settings")]
    public Transform chestSpawnPoint;
    public Transform coinSpawnPointA;
    public Transform coinSpawnPointB;
    public float chestSpawnRadius = 2f;
    public float coinSpawnRadius = 5f;

    public List<GameObject> treasures = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        Vector3 randomOffset = Random.insideUnitSphere * chestSpawnRadius;
        randomOffset.y = 0.5f;
        Vector3 spawnPosition = chestSpawnPoint.position + randomOffset;

        Instantiate(chestPrefab, spawnPosition, Quaternion.identity, transform);

        for(int i = 0; i < 2; i++)
        {
            Transform basePoint = Random.value < 0.5f ? coinSpawnPointA : coinSpawnPointB;

            Vector3 randomOffset1 = Random.insideUnitSphere * coinSpawnRadius;
            randomOffset1.y = 0.5f;

            Vector3 spawnPosition1 = basePoint.position + randomOffset1;
            Instantiate(coinPrefab, spawnPosition1, Quaternion.identity, transform);
        }
    }

    public List<GameObject> GetTreasures()
    {
        treasures.Clear();
        foreach (Transform child in transform)
        {
            if(child.CompareTag("Coin") || child.CompareTag("Chest"))
                treasures.Add(child.gameObject);
        }
        return treasures;
    }

    public void DestroyChest()
    {
        StartCoroutine(DestroyAndRespawnTreasure());
    }

    public IEnumerator DestroyAndRespawnTreasure()
    {
        yield return new WaitForSeconds(5f);

        if (chestPrefab != null && chestSpawnPoint != null)
        {
            Vector3 randomOffset = Random.insideUnitSphere * chestSpawnRadius;
            randomOffset.y = 0.5f;
            Vector3 spawnPosition = chestSpawnPoint.position + randomOffset;

            Instantiate(chestPrefab, spawnPosition, Quaternion.identity, transform);
        }
    }

    public void DestroyCoin()
    {
        StartCoroutine(DestroyAndRespawnCoin());
    }

    public IEnumerator DestroyAndRespawnCoin()
    {
        yield return new WaitForSeconds(5f);

        if (coinPrefab == null) yield break;

        Transform basePoint = Random.value < 0.5f ? coinSpawnPointA : coinSpawnPointB;
        if (basePoint == null)
        {
            Debug.LogWarning(" Не заданы точки спавна монет!");
            yield break;
        }

        Vector3 randomOffset = Random.insideUnitSphere * coinSpawnRadius;
        randomOffset.y = 0.5f;

        Vector3 spawnPosition = basePoint.position + randomOffset;
        Instantiate(coinPrefab, spawnPosition, Quaternion.identity, transform);
    }

    public void DropChest(Transform transformPlayer)
    {
        Vector3 position = transformPlayer.position;
        position.y = 0.5f;
        Instantiate(chestPrefab, position, Quaternion.identity, transform);
    }

    public void DropCoin(Transform transformPlayer)
    {
        Vector3 position = transformPlayer.position;
        position.y = 0.5f;
        Instantiate(coinPrefab, position, Quaternion.identity, transform);
    }
}