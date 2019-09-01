using System;
using Azure.AppServices;

[Serializable]
public class AgentInfo : DataModel
{
    public string AgentName;
    public float DeltaTime;
    public float ResourcePriority;
    public float ResourceValue;
    public float PathValues;
    public float ContinuousMovement;
    public float EnemyHits;
    public string CurrentState;
}
