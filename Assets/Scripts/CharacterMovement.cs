using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{

    public Transform Target;
    public float UpdateSpeed = 0.1f;

    private NavMeshAgent Agent;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateSpeed);

        while (enabled)
        {
            if(Target != null)
                Agent.SetDestination(Target.transform.position);
             else
            {
                // Логика, если цель отсутствует, например, остановка агента
                Agent.ResetPath();
                yield break;
            }
                
            yield return Wait;
        }
    }

}
