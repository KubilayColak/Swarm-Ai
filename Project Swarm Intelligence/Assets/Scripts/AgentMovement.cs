using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class AgentMovement : MonoBehaviour
{

    public TestClientConnection test;
    Renderer thisRend;

    float distance = 100f;
    float radius = 20;
    float Timer;
    float stuckTime;
    float currDelta;
    float prevDelta;
    float deltaThree;

    public float speed = 5;
    public float HP = 100f;
    public float Sensorlegth = 1.0f;
    public float Power;
    public float Limit;

    public bool DoOnce;
    public bool Task;

    //analytics 
    float PathValue;
    public float ResourceValue;
    public float MoveingValue;
    public string theState;
    public float HitValue = 1f;
    float GameTime;
    //end

    Vector3 vel;
    float currentPathValue;
    public float PowerLimit = 10;

    public GameObject Hive;
    public Transform Enemy;
    public Vector3 localScale;
    public Image LoadBar;

    Animator anim;
    NavMeshAgent agent;
    SphereCaster Identify;
    public ResourceStats HarvestInfo;
    EnemyMovement enemy;
    Transform TargetDestination;
    AgentData pathvalue;

    private enum States { CheckHive, Scouting, HarvestResource, HarvestHive, Defend }

    [SerializeField]
    private States currState;
    
    void Start()
    {
        pathvalue = GetComponent<AgentData>();
        Identify = GetComponent<SphereCaster>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemy = GetComponent<EnemyMovement>();
        Timer = 20;
        InvokeRepeating("Idle", 1f, Random.Range(30f, 60f));
        test = GameObject.FindWithTag("Connection").GetComponent<TestClientConnection>();
        thisRend = GetComponent<Renderer>();
    }

    void Update()
    {
        vel = agent.velocity;
        agent.speed = speed;
        GameTime += Time.deltaTime;
        
        if(vel.magnitude == 0)
        {
            stuckTime += Time.deltaTime;
            if(stuckTime >= 10)
            {
                currState = States.Scouting;
                stuckTime = 0;
            }
        }

        if (Identify.currentHitObject)
        {
            if (Identify.currentHitObject.tag == "Tasks")
            {
                Task = true;
                HarvestInfo = Identify.currentHitObject.GetComponent<ResourceStats>();
            }
            if (Identify.currentHitObject.tag == "Enemy")
            {
                currState = States.Defend;
            }
        }
        if (Task)
        {
            if (Identify.currentHitObject)
            {
                if (Identify.currentHitObject.tag == "Tasks" || Identify.currentHitObject.tag == "Hive")
                {
                    TargetDestination = Identify.currentHitObject.transform;
                }
            }
            if (TargetDestination != null)
            {
                currDelta = Vector3.Distance(TargetDestination.position, agent.transform.position);
            }
            deltaThree = agent.speed;

            PathValue = deltaThree / (prevDelta - currDelta);

            if ((prevDelta - deltaThree) < currDelta)
            {
                PathValue = -1;
            }
        }
        else if (!Task)
        {
            HarvestInfo = null;
            Task = false;
        }

        switch (currState)
        {
            case States.CheckHive:
                {
                    theState = "CheckHive";
                    thisRend.material.SetColor("_Color", Color.gray);
                    Idle();
                    break;
                }
            case States.Scouting:
                {
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isSearch", true);
                    theState = "Scouting";
                    thisRend.material.SetColor("_Color", Color.yellow);
                    MoveMent();
                    break;
                }
            case States.HarvestResource:
                {
                    anim.SetBool("isHarvest", true);
                    anim.SetBool("isSearch", false);
                    anim.SetBool("isDefend", false);
                    theState = "HarvestResource";
                    thisRend.material.SetColor("_Color", Color.green);
                    Harvesting();
                    break;
                }
            case States.HarvestHive:
                {
                    anim.SetBool("isHarvest", true);
                    anim.SetBool("isSearch", false);
                    anim.SetBool("isDefend", false);
                    theState = "HarvestHive";
                    thisRend.material.SetColor("_Color", Color.green);
                    HarvestingHive();
                    break;
                }
            case States.Defend:
                {
                    anim.SetBool("isDefend", true);
                    anim.SetBool("isHarvest", false);
                    anim.SetBool("isSearch", false);
                    theState = "Defend";
                    thisRend.material.SetColor("_Color", Color.red);
                    Defending();
                    break;
                }
        }
    }

    void Idle()
    {
        if (GameTime >= 1 && !test.isClosed)
        {
            test.agentName.Add(pathvalue.AgentName);
            test.deltaTime.Add(Time.time);
            test.pathValue.Add(PathValue);
            test.resourceValue.Add(ResourceValue);
            test.continuousMovement.Add(MoveingValue);
            test.hits.Add(HitValue);
            test.currentState.Add(theState);
            test.resourcePriority.Add(0);
            GameTime = 0;
        }
        currState = States.CheckHive;
        agent.SetDestination(Hive.transform.position);
        if (Identify.currentHitObject)
        {
            if (Identify.currentHitObject.tag == "Hive")
            {
                currState = States.Scouting;
                Power = 0;
                LoadBar.fillAmount = Power;
            }
        }
        if (vel.magnitude == 0)
        {
            MoveingValue = -1;
        }
        else
        {
            MoveingValue = 1;
        }
    }

    void MoveMent()
    {
        if (GameTime >= 1 && !test.isClosed)
        {
            test.agentName.Add(pathvalue.AgentName);
            test.deltaTime.Add(Time.time);
            test.pathValue.Add(PathValue);
            test.resourceValue.Add(ResourceValue);
            test.continuousMovement.Add(MoveingValue);
            test.hits.Add(HitValue);
            test.currentState.Add(theState);
            test.resourcePriority.Add(0);
            GameTime = 0;
        }
        Vector3 newPos = Random.insideUnitCircle * radius;
        Vector3 Walking = new Vector3(newPos.x, 0, newPos.y) * distance;
        agent.SetDestination(Walking);
        if (Task == true)
        {
            currState = States.HarvestResource;
            anim.SetBool("isHarvest", true);
        }
        if (vel.magnitude == 0)
        {
            MoveingValue = -1;
        }
        else
        {
            MoveingValue = 1;
        }
    }

    void Harvesting()
    {
        if (GameTime >= 1 && !test.isClosed)
        {
            test.agentName.Add(pathvalue.AgentName);
            test.deltaTime.Add(Time.time);
            test.pathValue.Add(PathValue);
            test.resourceValue.Add(ResourceValue);
            test.continuousMovement.Add(MoveingValue);
            test.currentState.Add(theState);
            test.hits.Add(HitValue);
            test.resourcePriority.Add(GetComponent<TaskInfo>().ResourcePriority);
            GameTime = 0;
        }
        if (vel.magnitude == 0)
        {
            MoveingValue = -1;
        }
        else
        {
            MoveingValue = 1;
        }

        if (Power >= PowerLimit)
        {
            currState = States.HarvestHive;
        }
       else
     
        {
            if (HarvestInfo != null)
            {
                agent.SetDestination(HarvestInfo.transform.position);
            }
            if (Identify.currentHitObject)
            {
                if (Identify.currentHitObject.tag == "Tasks")
                {
                    Invoke("Scale", 1);
                    Timer += Time.deltaTime;
                    if (Timer > 20 && HarvestInfo.resourceEnergy < 10)
                    {
                        prevDelta = Vector3.Distance(Hive.transform.position, transform.position);
                        PowerLimit = HarvestInfo.resourceEnergy;
                        Timer = 0;
                        HarvestInfo.EnergyValue--;
                    }
                    else
                    {
                        prevDelta = Vector3.Distance(Hive.transform.position, transform.position);
                        Power++;
                        HarvestInfo.resourceEnergy--;
                        LoadBar.fillAmount = Power / PowerLimit;
                        HarvestInfo.EnergyValue--;
                    }
                    if (HarvestInfo.resourceEnergy >= 10)
                    {
                        PowerLimit = 10;
                    }
                }
            }
        }
        if (HarvestInfo == null)
        {
            currState = States.CheckHive;
            HarvestInfo = null;
            Task = false;
        }
    }

    void HarvestingHive()
    {
        if (GameTime >= 1 && !test.isClosed)
        {
            test.agentName.Add(pathvalue.AgentName);
            test.deltaTime.Add(Time.time);
            test.pathValue.Add(PathValue);
            test.resourceValue.Add(ResourceValue);
            test.continuousMovement.Add(MoveingValue);
            test.currentState.Add(theState);
            test.hits.Add(HitValue);
            test.resourcePriority.Add(GetComponent<TaskInfo>().ResourcePriority);
            GameTime = 0;
        }
        if (Power >= 10)
        {
            ResourceValue = 1;
        }
        else if (Power < 10)
        {
            ResourceValue = (Power - 10f) / 10f;
        }
        agent.SetDestination(Hive.transform.position);

        if (Identify.currentHitObject)
        {
            if (Identify.currentHitObject.name == "Hive")
            {
                prevDelta = Vector3.Distance(HarvestInfo.transform.position, transform.position);

                Power = 0;
                HP -= Random.Range(5, 20);
                if (HP <= 10)
                {
                    HP += 110;
                }
                LoadBar.fillAmount = Power;
                currState = States.HarvestResource;
            }
        }
        if (HarvestInfo == null)
        {
            currState = States.CheckHive;
            HarvestInfo = null;
            Task = false;
        }
    }

    void Defending()
    {
        if (GameTime >= 1 && !test.isClosed)
        {
            test.agentName.Add(pathvalue.AgentName);
            test.deltaTime.Add(Time.time);
            test.pathValue.Add(PathValue);
            test.resourceValue.Add(ResourceValue);
            test.continuousMovement.Add(MoveingValue);
            test.hits.Add(HitValue);
            test.currentState.Add(theState);
            test.resourcePriority.Add(0);
            GameTime = 0;
        }
        agent.SetDestination(Hive.transform.position);
        agent.transform.LookAt(Enemy);
        Timer += Time.deltaTime;
        if (Identify.currentHitObject)
        {
            if (Identify.currentHitObject.tag != "Enemy" && Timer >= 10)
            {
                currState = States.Scouting;
            }
        }
    }

    void OnApplicationQuit()
    {
        GameOver();
    }

    public void GameOver()
    {
        if (!test.isClosed)
        {
            test.agentName.Add(pathvalue.AgentName);
            test.deltaTime.Add(Time.time);
            test.pathValue.Add(PathValue);
            test.resourceValue.Add(ResourceValue);
            test.continuousMovement.Add(MoveingValue);
            test.currentState.Add(theState);
            test.resourcePriority.Add(GetComponent<TaskInfo>().ResourcePriority);
        }
    }

    void Scale()
    {
        if (HarvestInfo != null)
        {
            HarvestInfo.transform.localScale -= new Vector3(0.002F, 0.002f, 0.002f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * (Sensorlegth + transform.localScale.z));
    }

    public void GetTask(ResourceStats stat)
    {
        HarvestInfo = stat;
    }
}
