using UnityEngine;

public class HTNAgentSensors : MonoBehaviour
{
    public GameObject actor;
    public HTNPlanner planner;
    protected Character character;

    void Start()
    {
        actor = gameObject;
        character = actor.GetComponent<Character>();
        Initialize(actor);
    }

    public void SetState(string stateKey, object value, bool notifyChanges = true)
    {
        planner.GetWorldState().SetValue(stateKey, value, notifyChanges);
    }

    public virtual void Initialize(GameObject actor)
    {
        
    }
}