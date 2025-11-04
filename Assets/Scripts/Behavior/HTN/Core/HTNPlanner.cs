using UnityEngine;
using System.Collections.Generic;

public class HTNPlanner : MonoBehaviour
{
    
    public Character character;
    public HTNCompoundTask rootTask;


    private HTNCompoundTask domain;
    private HTNWorldState worldState;
    private PlanRunner planRunner;
    private List<HTNWorldState> stateStack = new List<HTNWorldState>();

    public void Awake()
    {
        character = gameObject.GetComponent<Character>();
        SetupWorldState();
        SetupDomain();
        SetupPlanRunner();
    }

    public void Start()
    {
        Plan();
    }
    public void Update()
    {
        planRunner.Process();
    }

    private void SetupWorldState()
    {
        worldState = new HTNWorldState();
        worldState.OnStateChanged += OnWorldStateChanged;
        Debug.Log("SetupWorldState");
    }

    private void SetupDomain()
    {
        if (rootTask == null)
        {
            Debug.LogError("Domain was not defined");
            return;
        }

        domain = rootTask;
    }

    private void SetupPlanRunner()
    {
        if (character == null)
        {
            Debug.LogError("Actor not defined");
            return;
        }

        planRunner = new PlanRunner(worldState, character);
        planRunner.PlanFinished += OnPlanExecutionFinished;
        Debug.Log("SetupPlanRunner");
    }

    private void Plan()
    {
        InitializeWorkingState();
        Debug.Log("Plan");
        List<HTNTask> plan = DecomposeCompoundTask(domain);

        if (plan.Count > 0)
        {
            Debug.Log("New Plan");
            string textPlan = "План: ";
            foreach (HTNTask task in plan)
            {
                textPlan += $"--> {task} ";
            }
            Debug.Log(textPlan);

            planRunner.SetPlan(plan);

        }
        ClearWorkingState();

    }

    private List<HTNTask> DecomposeCompoundTask(HTNCompoundTask task)
    {
        var methods = task.GetMethods();
        HTNWorldState currentState = CurrentWorkingState();

        foreach (var method in methods)
        {
            if (CheckConditions(method.PreConditions, currentState))
            {
                SaveCurrentWorkingState();
                List<HTNTask> decomposedTasks = DecomposeTask(method.Tasks);

                if (decomposedTasks.Count == 0)
                {
                    RestorePreviousState();
                    continue;
                }
                return decomposedTasks;
            }
        }
        return new List<HTNTask>();
    }

    private List<HTNTask> DecomposeTask<T>(List<T> tasks) where T : HTNTask
    {
        List<HTNTask> decomposedTasks = new List<HTNTask>();
        HTNWorldState currentState = CurrentWorkingState();

        foreach (var task in tasks)
        {
            if (task is HTNCompoundTask compoundTask)
            {
                List<HTNTask> t = DecomposeCompoundTask(compoundTask);
                if (t.Count == 0)
                    return new List<HTNTask>();
                decomposedTasks.AddRange(t);
            }
            else if (task.IsAvailable(currentState))
            {
                task.ApplyEffects(currentState);
                decomposedTasks.Add(task);
            }
            else return new List<HTNTask>();
        }
        return decomposedTasks;
    }


    private bool CheckConditions (Dictionary<string, object> conditions, HTNWorldState state)
    {
        foreach (string key in conditions.Keys)
        {
            if (!state.GetValue(key).Equals(conditions[key]))
                return false;
        }
        return true;
    }


    private void OnWorldStateChanged()
    {
        Debug.Log("World state changed. Re-planning.");
        Plan();
    }

    private void OnPlanExecutionFinished()
    {
        Debug.Log("Plan finished. Re-planning.");
        Plan();
    }

    public HTNWorldState GetWorldState()
    {
        return worldState;
    }

    private void InitializeWorkingState()
    {
        stateStack.Add(worldState.Duplicate());
    }

    private HTNWorldState CurrentWorkingState()
    {
        return stateStack[stateStack.Count - 1];
    }

    private void SaveCurrentWorkingState()
    {
        stateStack.Add(CurrentWorkingState().Duplicate());
    }

    private void RestorePreviousState()
    {
        stateStack.RemoveAt(stateStack.Count - 1);
    }

    private void ClearWorkingState()
    {
        stateStack.Clear();
    }

}
