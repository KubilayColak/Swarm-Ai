using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

    public GameObject panel;

	// Use this for initialization
	void Start () {
        Time.timeScale = 2;
	}
	
	// Update is called once per frame
	void Update () {
		if(FindClosestTask() == null)
        {
            panel.SetActive(true);
            GameObject.FindWithTag("Connection").GetComponent<TestClientConnection>().isClosed = true;
        }
	}

    public GameObject FindClosestTask()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Tasks");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
