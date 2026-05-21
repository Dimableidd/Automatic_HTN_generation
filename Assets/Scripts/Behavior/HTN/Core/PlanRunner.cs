using System;
using System.Collections.Generic;
using UnityEngine;

public class PlanRunner
{
    public event Action PlanFinished;

    private HTNWorldState worldState;
    private List<HTNTask> tasks;
    private Character character;

    private int lastAction = -1;
    private int[] lastState = new int[4] { -1, -1, -1, -1 };

    public PlanRunner(HTNWorldState _worldState, Character _character)
    {
        worldState = _worldState;
        character = _character;
    }

    public void SetPlan(List<HTNTask> _tasks)
    {
        tasks = _tasks;
        Debug.Log("New plan set. Tasks: " + tasks.Count);

        if (tasks != null && tasks.Count > 0)
        {
            LogDecision(tasks[0]);
        }
    }

    public void Process()
    {
        if (tasks.Count == 0)
            return;
        ExecuteCurrentTask();
    }

    public void ExecuteCurrentTask()
    {
        HTNTask currentTask = tasks[0];
        HTNTask.TaskResult taskResult = currentTask.Execute(character);

        switch (taskResult)
        {
            case HTNTask.TaskResult.PROCESSING:
                break;
            case HTNTask.TaskResult.FAILURE:
                Debug.Log("Task failed  " + currentTask);
                FinishPlan();
                break;
            case HTNTask.TaskResult.SUCCESS:
                Debug.Log("Task succeeded  " + currentTask);
                NextTask();
                break;
        }
    }

    public void NextTask()
    {
        tasks = PopFront();

        if(tasks.Count == 0)
        {
            FinishPlan();
            return;
        }

        HTNTask newTask = tasks[0];

        if (!newTask.IsAvailable(worldState))
        {
            Debug.Log("Task not available  " + newTask);
            FinishPlan();
            return;
        }

        LogDecision(newTask);

        Debug.Log("Starting task  "+ newTask);
        ExecuteCurrentTask();
    }

    private void LogDecision(HTNTask task)
    {
        int action = MapTaskToAction(task);
        int[] state = GetStateFromWorldState();

        if (IsDifferent(state, action))
        {
            GlobalManager.Instance.LogAction(
                false,
                character.GetInstanceID(),
                state[0],
                state[1],
                state[2],
                state[3],
                action
            );

            UpdateLast(state, action);
        }
    }

    private int[] GetStateFromWorldState()
    {
        return new int[4]
        {
            ConvertToInt(worldState.GetValue("hasTreasure")),
            ConvertToInt(worldState.GetValue("enemyVisible")),
            ConvertToInt(worldState.GetValue("enemyInRange")),
            ConvertToInt(worldState.GetValue("treasureOnMap"))
        };
    }

    private int ConvertToInt(object value)
    {
        if (value == null) return 0;

        if (value is bool b)
            return b ? 1 : 0;

        return Convert.ToInt32(value);
    }

    private bool IsDifferent(int[] currentState, int action)
    {
        if (action != lastAction)
            return true;

        for (int i = 0; i < 4; i++)
        {
            if (currentState[i] != lastState[i])
                return true;
        }

        return false;
    }

    private void UpdateLast(int[] state, int action)
    {
        lastAction = action;

        for (int i = 0; i < 4; i++)
            lastState[i] = state[i];
    }

    private int MapTaskToAction(HTNTask task)
    {
        string name = task.GetType().Name;

        if (name.Contains("Task_Idle")) return 0;
        if (name.Contains("Task_GoToTreasure")) return 1;
        if (name.Contains("Task_GoToBase")) return 2;
        if (name.Contains("Task_GoToEnemy")) return 3;
        if (name.Contains("Task_Attack")) return 4;

        Debug.LogWarning("Unknown task mapping: " + name);
        return -1;
    }

    public List<HTNTask> PopFront()
    {
        if (tasks.Count == 0)
        {
            return null;
        }

        tasks.RemoveAt(0);

        return tasks;
    }

    public void FinishPlan()
    {
        tasks = new List<HTNTask>();
        PlanFinished?.Invoke();
    }
}