using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    public static EnemyList Instance;
    private void Awake()
    {
        Instance = this;
    }

    public List<GameObject> _clonesList = new();
    public void DeleteAllClones()
    {
        foreach (GameObject clone in _clonesList)
        {
            Destroy(clone);
            Debug.Log("destroyed");
        }
        _clonesList.Clear();
    }
    public void AddCloneToList(GameObject clone)
    {
        Debug.Log(clone.name + " added");
        _clonesList.Add(clone);
        
    }
}