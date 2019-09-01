using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceStats : MonoBehaviour
{
    
    public string Title;
    public string Type;
    public int resourceEnergy;
    public float EnergyValue = 100f;
    public bool addList;

    void Update()
    {
        if (resourceEnergy <= 0)
        {
           Destroy(gameObject);
        }
    }
}
