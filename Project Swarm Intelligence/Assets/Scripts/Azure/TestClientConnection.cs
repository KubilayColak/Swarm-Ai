using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Azure.AppServices;
using RESTClient;
using System;
using System.Net;
using UnityEngine.UI;
using Tacticsoft;
using UnityEngine.SceneManagement;
using System.Linq;

public class TestClientConnection : MonoBehaviour
{

    public List<string> agentName = new List<string>();
    public List<float> deltaTime = new List<float>();
    public List<float> pathValue = new List<float>();
    public List<float> resourceValue = new List<float>();
    public List<float> resourcePriority = new List<float>();
    public List<float> continuousMovement = new List<float>();
    public List<float> hits = new List<float>();
    public List<string> currentState = new List<string>();
    string name;
    float delta;
    float path;
    float resource;
    float priority;
    float Movement;
    float HitValue;
    string States;

    [SerializeField]
    private string _appUrl = "https://unity-tables.azurewebsites.net/";

    private AppServiceClient _client;

    // App Service Table defined using a DataModel
    private AppServiceTable<AgentInfo> _table;

    public bool isClosed = false;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Connection");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        // Create App Service client
        _client = new AppServiceClient(_appUrl);

        // Get App Service 'Highscores' table
        _table = _client.GetTable<AgentInfo>("AgentInfo");
    }

    public void Insert()
    {
        AgentInfo newInfo = new AgentInfo();

        for (int i = 0; i < agentName.Count; i++)
        {
            name = agentName[i];
            delta = deltaTime[i];
            path = pathValue[i];
            resource = resourceValue[i];
            priority = resourcePriority[i];
            Movement = continuousMovement[i];
            HitValue = hits[i];
            States = currentState[i];

            newInfo.AgentName = name;
            newInfo.DeltaTime = delta;
            newInfo.PathValues = path;
            newInfo.ResourceValue = resource;
            newInfo.ResourcePriority = priority;
            newInfo.ContinuousMovement = Movement;
            newInfo.EnemyHits = HitValue;
            newInfo.CurrentState = States;

            StartCoroutine(_table.Insert<AgentInfo>(newInfo, OnInsertCompleted));
        }
    }

    private void OnInsertCompleted(IRestResponse<AgentInfo> response)
    {
        if (!response.IsError && response.StatusCode == HttpStatusCode.Created)
        {
            Debug.Log("OnInsertItemCompleted: " + response.Content + " status code:" + response.StatusCode + " data:" + response.Data);
        }
        else
        {
            Debug.LogWarning("Insert Error Status:" + response.StatusCode + " Url: " + response.Url);
        }
    }
}

