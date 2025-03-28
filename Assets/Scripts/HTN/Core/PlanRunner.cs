using System;
using System.Collections.Generic;
using UnityEngine;

public class PlanRunner
{
    public event Action PlanFinished;

    private HTNWorldState worldState;
    private List<HTNTask> tasks;
    private GameObject actor;

    public PlanRunner(HTNWorldState _worldState, GameObject _actor)
    {
        worldState = _worldState;
        actor = _actor;
    }

    public void SetPlan(List<HTNTask> _tasks)
    {
        tasks = _tasks;
        Debug.Log("New plan set. Tasks: " + tasks.Count);
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
        HTNTask.TaskResult taskResult = currentTask.Execute(actor);

        switch (taskResult)
        {
            case HTNTask.TaskResult.PROCESSING:
                return;
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
        Debug.Log(worldState.state);
        if (!newTask.IsAvailable(worldState))
        {
            Debug.Log("Task not available  " + newTask);
            FinishPlan();
            return;
        }

        Debug.Log("Starting task  "+ newTask);
        ExecuteCurrentTask();
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