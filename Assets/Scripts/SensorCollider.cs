using System;
using UnityEngine;

public class SensorCollider : MonoBehaviour
{
    public Character character;

    public void Awake()
    {
        character = gameObject.GetComponentInParent<Character>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Chest")) return;
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Character>().team != character.team)
        {
            character.AddEnemy(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Character>().team != character.team)
        {
            character.RemoveEnemy(other.gameObject);
        }
    }
}
