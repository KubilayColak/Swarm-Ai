using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    float radius = 10f;
    float lengthOfpath = 100f;
    NavMeshAgent EnemyAgent;
    AgentMovement AgentTarget;
    SphereCaster Myeyes;

    // Use this for initialization
    void Start()
    { 
        AgentTarget = GetComponent<AgentMovement>();
        EnemyAgent = GetComponent<NavMeshAgent>();
        Myeyes = GetComponent<SphereCaster>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Random.insideUnitCircle * radius;
        Vector3 SearchPrey = new Vector3(newPos.x, 0, newPos.y) * lengthOfpath;
        EnemyAgent.SetDestination(SearchPrey);

        if (Myeyes.currentHitObject != null)
        { 
            EnemyAgent.SetDestination(Myeyes.currentHitObject.transform.position);
            if (Myeyes.currentHitObject.tag == "Agent")
            {
                Destroy(gameObject);
                AgentTarget = Myeyes.currentHitObject.GetComponent<AgentMovement>();
                AgentTarget.HitValue -= 0.1f;
                if(AgentTarget.Power > 0)
                {
                    AgentTarget.Power -= 1f;
                }
            }
        }
    }
}

