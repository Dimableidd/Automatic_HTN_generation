using UnityEngine;

public class TriggerCoin : MonoBehaviour
{
    public GameManager gameManager;
    private bool _taken = false;
    public void Awake()
    {
        gameManager = transform.parent.GetComponent<SpawnTrasures>().gameManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_taken) return;

        if (other.CompareTag("Player"))
        {
            Character player = other.GetComponent<Character>();
            if (player != null && !player.boolChest && !player.boolCoin)
            {
                _taken = true;

                player.boolCoin = true;
                player.SpawnIcon(player.coinIconPrefab);

                if (gameManager.learning)
                {
                    RL_Agent agent = other.GetComponent<RL_Agent>();
                    if (agent != null)
                        agent.AddRewardUpTreashure();
                }

                Destroy(gameObject);
            }
        }
    }
}
