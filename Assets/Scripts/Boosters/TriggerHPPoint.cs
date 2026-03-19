using UnityEngine;

public class TriggerHPPoint : MonoBehaviour
{
    public GameManager gameManager;
    public House house;
    public bool _taken = false;
    public void Awake()
    {
        gameManager = house.transform.parent.GetComponent<Team>().gameManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_taken) return;

        if (other.CompareTag("Player"))
        {
            Character player = other.GetComponent<Character>();
            if (player != null && house.teamName == player.team)
            {
                _taken = true;

                player.currentHealth = player.maxHealth;

                if (gameManager.learning)
                {
                    RL_Agent agent = other.GetComponent<RL_Agent>();
                    if (agent != null)
                        agent.AddRewardUpBooster();
                }

                house.SpawnHP();
                gameObject.SetActive(false);
            }
        }
    }
}
