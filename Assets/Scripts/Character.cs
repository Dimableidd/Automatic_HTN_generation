using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;
using UnityEngine.AI;
using Unity.VisualScripting;

public class Character : MonoBehaviour
{
    [SerializeField] public int currentHealth = 20;
    [SerializeField] public int Damage = 1;

    [SerializeField] public int team;

    public GameObject Treasure;
    public bool boolChest = false;
    public bool boolCoin = false;

    public Transform HomeBase => transform.parent.GetComponentInChildren<House>().transform;

    public bool isAttack = false;
    public float attackDist = 2f;



    // Префаб для отображения иконки над головой
    public GameObject chestIconPrefab;
    public GameObject coinIconPrefab;

    public event Action TargetAction;
    public Transform Target;
    public float UpdateSpeed = 0.1f;
    public float attackSpeed = 1f;
    private float lastAttackTime = 0f; // Время последней атаки
    public NavMeshAgent Agent;

    public List<GameObject> enemy = new List<GameObject>();


    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(Transform target)
    {
        Target = target;
        TargetAction?.Invoke();
    }
    public void AddEnemy(GameObject target)
    {
        enemy.Add(target);
    }

    public void RemoveEnemy(GameObject target)
    {
        enemy.Remove(target);
    }

    public bool CanAttack()
    {
        float timeSinceLastAttack = Time.time - lastAttackTime;
        return timeSinceLastAttack >= 1f / attackSpeed;
    }

    public void Attack (GameObject target)
    {
        if (Vector3.Distance(gameObject.transform.position, target.transform.position) <= attackDist )
            StartCoroutine(AttackTarget(target.GetComponent<Character>()));
    }

    public void TakeDamage ()
    {
        currentHealth -= Damage;
        if(currentHealth <= 0)
        {
            gameObject.GetComponentInParent<Team>().StartDestroyCharacter();
            if(boolChest)
            {
                boolChest = false;
                Destroy(Treasure);
                SpawnTrasures.Instance.DropChest(transform);
            }
            if(boolCoin)
            {
                boolCoin = false;
                Destroy(Treasure);
                SpawnTrasures.Instance.DropCoin(transform);
            }
            DeathCharacter();
        }
        
    }

    public void DeathCharacter()
    {
        foreach (GameObject target in enemy)
        {
            target.GetComponent<Character>().enemy.Remove(gameObject);
        }
        Destroy(gameObject);
    }

    public IEnumerator AttackTarget(Character target)
    {

        if (target == null)
            yield return null;

        target.TakeDamage();

        // Обновляем время последней атаки
        lastAttackTime = Time.time;

        // Ждем перед следующей атакой
        float attackCooldown = 1f / attackSpeed;
        yield return new WaitForSeconds(attackCooldown);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (!boolChest && !boolCoin)
        {
            // Проверяем, есть ли у объекта, с которым произошло столкновение, тег "Chest"
            if (other.CompareTag("Chest"))
            {
                boolChest = true;
                Destroy(other.gameObject);
                SpawnIcon(chestIconPrefab);
                Physics.IgnoreCollision(GetComponent<Collider>(), other, true);
            }
            // Проверяем, есть ли у объекта, с которым произошло столкновение, тег "Coin"
            else if (other.CompareTag("Coin"))
            {
                boolCoin = true;
                Destroy(other.gameObject);
                SpawnIcon(coinIconPrefab);
                Physics.IgnoreCollision(GetComponent<Collider>(), other, true);
            }
        }
        else if (other.CompareTag("House") && other.gameObject.GetComponent<House>().teamName == team)
        {
            if (boolChest)
            {
                gameObject.GetComponentInParent<Team>().AddScore(500);
                boolChest = false;
                Destroy(Treasure);
                SpawnTrasures.Instance.DestroyChest();
            }
            else if (boolCoin)
            {
                gameObject.GetComponentInParent<Team>().AddScore(250);
                boolCoin = false;
                Destroy(Treasure);
                SpawnTrasures.Instance.DestroyCoin();
            }
        }
    }

    public void SpawnIcon(GameObject iconPrefab)
    {
        if (iconPrefab != null)
        {
            // Создаём экземпляр иконки
            GameObject iconInstance = Instantiate(iconPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
            Treasure = iconInstance;
            // Получаем компонент FloatingIcon
            FloatingIcon floatingIcon = iconInstance.GetComponent<FloatingIcon>();
            if (floatingIcon != null)
            {
                // Устанавливаем цель для иконки
                floatingIcon.SetTarget(this.transform);
            }
        }
    }
        
    public GameObject GetNearestTreasure()
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

    public GameObject GetNearestEnemy()
    {
        float min = float.MaxValue;
        GameObject nearest = null;
        foreach (var e in enemy)
        {
            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < min) { min = d; nearest = e; }
        }
        return nearest;
    }

}
