using UnityEngine;

public class TriggerChest : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Character player = other.GetComponent<Character>();
            if (!player.boolChest && !player.boolCoin)
            {
                player.boolChest = true;
                player.SpawnIcon(player.chestIconPrefab);
                Destroy(gameObject);
            }
        }
    }
}
