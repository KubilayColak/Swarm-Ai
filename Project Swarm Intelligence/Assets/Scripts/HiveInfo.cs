using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HiveInfo : MonoBehaviour {

    public List<GameObject> globalLocation = new List<GameObject>();

    // Update is called once per frame
    void Update ()
    {
        for (var i = globalLocation.Count - 1; i > -1; i--)
        {
            if (globalLocation[i] == null)
                globalLocation.RemoveAt(i);
        }
    }

    public void AddItem(List<GameObject> agentlist)
    {
        foreach (var item in agentlist)
        {
            if (!globalLocation.Contains(item))
            {
                globalLocation.Add(item);
            }
        }
    }
}

