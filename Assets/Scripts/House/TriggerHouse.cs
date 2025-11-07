using UnityEngine;

public class TriggerHouse : MonoBehaviour
{
    public GameManager gameManager;
    public SpawnTrasures spawnTrasures;

    public void Awake()
    {
        spawnTrasures = transform.parent.GetComponent<Team>().spawnTrasures;
        gameManager = transform.parent.GetComponent<Team>().gameManager;
    }
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
                    spawnTrasures.DestroyChest();
                    if(gameManager.learning)
                    {
                        if(other.GetComponent<RL_Agent>())
                            other.GetComponent<RL_Agent>().AddRewardDownTreashure();
                    }
                }
                else if (player.boolCoin)
                {
                    player.GetComponentInParent<Team>().AddScore(250);
                    player.boolCoin = false;
                    Destroy(player.Treasure);
                    spawnTrasures.DestroyCoin();
                    if(gameManager.learning)
                    {
                        if(other.GetComponent<RL_Agent>())
                            other.GetComponent<RL_Agent>().AddRewardDownTreashure();
                    }
                }
            }
        }
    }
}
