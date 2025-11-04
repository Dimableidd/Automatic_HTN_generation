using UnityEngine;

public class TriggerCoin : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Character player = other.GetComponent<Character>();
            if (!player.boolChest && !player.boolCoin)
            {
                player.boolCoin = true;
                player.SpawnIcon(player.coinIconPrefab);
                Destroy(gameObject);
            }
        }
    }
}
