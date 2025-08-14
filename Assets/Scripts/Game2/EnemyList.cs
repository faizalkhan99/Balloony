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
        }
        _clonesList.Clear();
    }
    public void AddCloneToList(GameObject clone)
    {
        _clonesList.Add(clone);
        
    }
}