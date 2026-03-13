using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class TriggerChest : MonoBehaviour
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

                player.boolChest = true;
                player.SpawnIcon(player.chestIconPrefab);
                player.GetComponent<NavMeshAgent>().speed = 2f;

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
