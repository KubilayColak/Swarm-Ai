using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    TestClientConnection con;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Connection"))
        {
            con = GameObject.FindGameObjectWithTag("Connection").GetComponent<TestClientConnection>();
        }
    }

    public void PlayGame()
    {
        if (con)
        {
            con.agentName.Clear();
            con.deltaTime.Clear();
            con.pathValue.Clear();
            con.resourceValue.Clear();
            con.resourcePriority.Clear();
            con.continuousMovement.Clear();
            con.hits.Clear();
            con.currentState.Clear();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
    
    public void MenuReturn()
    {
        con.Insert();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
	
}
