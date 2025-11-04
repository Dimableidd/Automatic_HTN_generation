using UnityEngine;

public class TriggerHouse : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Character player = other.GetComponent<Character>();
            if (gameObject.GetComponent<House>().teamName == player.team)
            {
                if (player.boolChest)
                {
                    player.GetComponentInParent<Team>().AddScore(250);
                    player.boolChest = false;
                    Destroy(player.Treasure);
                    SpawnTrasures.Instance.DestroyChest();
                }
                else if (player.boolCoin)
                {
                    player.GetComponentInParent<Team>().AddScore(250);
                    player.boolCoin = false;
                    Destroy(player.Treasure);
                    SpawnTrasures.Instance.DestroyCoin();
                }
            }
        }
    }
}
