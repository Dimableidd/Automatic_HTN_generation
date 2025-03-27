using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnTrasures : MonoBehaviour
{
    public static SpawnTrasures Instance;
    public GameObject chestPrefab; // Новый префаб для восстановления сокровищ
    public GameObject coinPrefab;  // Новый префаб для восстановления монет

    public List<GameObject> treasures = new List<GameObject>();


    void Awake()
    {
        Instance = this;
    }

    public List<GameObject> GetTreasures()
    {
        if (treasures.Count != 0) treasures.Clear();
        foreach (Transform child in transform)
        {
            treasures.Add(child.gameObject);
        }
        return treasures;
    }

    public GameObject FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }

            // Рекурсивный поиск в дочерних объектах
            GameObject result = FindChildWithTag(child, tag);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    public void DestroyChest()
    {
        StartCoroutine(DestroyAndRespawnTreasure());
    }

    public void DestroyCoin()
    {
        StartCoroutine(DestroyAndRespawnCoin());
    }
    public IEnumerator DestroyAndRespawnTreasure()
    {
        yield return new WaitForSeconds(5f); // Ждем 5 секунд
        // Восстанавливаем сокровище на изначальной позиции
        if (chestPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(0f, 0.5f, 0f);
            Instantiate(chestPrefab, spawnPosition, Quaternion.identity, transform);
        }
    }

    public IEnumerator DestroyAndRespawnCoin()
    {
        yield return new WaitForSeconds(5f); // Ждем 5 секунд
        // Восстанавливаем монету на изначальной позиции
        
        GameObject foundChild = FindChildWithTag(this.transform, "Coin");

        if (coinPrefab != null && foundChild != null)
        {
            if(foundChild.transform.position == new Vector3(20f, 0.5f, 0f))
            {
                if(foundChild.transform.position != new Vector3(-20f, 0.5f, 0f))
                    Instantiate(coinPrefab, new Vector3(-20f, 0.5f, 0f), Quaternion.identity, transform);
            }
            else
                Instantiate(coinPrefab, new Vector3(20f, 0.5f, 0f), Quaternion.identity, transform);
        }
        else
            Instantiate(coinPrefab, new Vector3(20f, 0.5f, 0f), Quaternion.identity, transform);
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
