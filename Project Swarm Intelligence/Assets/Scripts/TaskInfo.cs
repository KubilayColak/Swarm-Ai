using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TaskInfo : MonoBehaviour
{
    public List<GameObject> listOfResources = new List<GameObject>();
    SphereCaster Position;
    HiveInfo hiveinfo;
    public ResourceStats currentResource;
    public ResourceStats highestResource;
    public float currentresourceEnergy = 0, highestresourceEnergy, ResourcePriority;

    void Start()
    {
        Position = GetComponent<SphereCaster>();
        hiveinfo = GetComponent<HiveInfo>();
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < listOfResources.Count; i++)
        {
            if (listOfResources[i])
            {
                if (listOfResources[i].GetComponent<ResourceStats>().EnergyValue >= currentresourceEnergy)
                {
                    highestResource = listOfResources[i].GetComponent<ResourceStats>();
                    highestresourceEnergy = highestResource.EnergyValue;
                }
            }
        }

        if (currentResource == null && GetComponent<AgentMovement>().HarvestInfo != null)
        {
            currentResource = GetComponent<AgentMovement>().HarvestInfo;
        }
        else if (currentResource != null)
        {
            currentresourceEnergy = currentResource.EnergyValue;
        }
      
        ResourcePriority = Mathf.Clamp01(currentresourceEnergy / highestresourceEnergy);
        

        for (var i = listOfResources.Count - 1; i > -1; i--)
        {
            if (listOfResources[i] == null)
                listOfResources.RemoveAt(i);
        }

        if (Position.currentHitObject)
        {
            if (Position.currentHitObject.GetComponent<ResourceStats>())
            {
                ResourceStats resouce = Position.currentHitObject.GetComponent<ResourceStats>();

                if (resouce.addList)
                {
                    //print("already in list");
                }
                else if (!resouce.addList)
                {
                    resouce.addList = true;
                    listOfResources.Add(Position.currentHitObject);
                }
            }
            else if (Position.currentHitObject.GetComponent<HiveInfo>())
            {
                hiveinfo = Position.currentHitObject.GetComponent<HiveInfo>();
                if (!GetComponent<AgentMovement>().Task)
                {
                    if (hiveinfo.globalLocation.Count != 0)
                    {
                        AddItem(hiveinfo.globalLocation);
                        for (int i = 0; i < listOfResources.Count; i++)
                        {
                            if (listOfResources[i].GetComponent<ResourceStats>().EnergyValue > 0)
                            {
                                GetComponent<AgentMovement>().GetTask(listOfResources[i].GetComponent<ResourceStats>());
                                GetComponent<AgentMovement>().Task = true;
                               return;
                            }
                        }
                        
                    }
                }
                else
                {
                    hiveinfo.AddItem(listOfResources);
                    AddItem(hiveinfo.globalLocation);
                }
            }
        }
        else { };

    }

    public void AddItem(List<GameObject> agentlist)
    {
        foreach (var item in agentlist)
        {
            if (!listOfResources.Contains(item))
            {
                listOfResources.Add(item);
            }
        }
    }
}
