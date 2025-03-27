using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] public Team team;

    public int teamName;

    void Start()
    {
        teamName = team.teamName;
    }

    
}
