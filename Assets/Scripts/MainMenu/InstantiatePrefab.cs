using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    void Instantiate(GameObject prefab)
    {
        Instantiate(prefab);
    }

    //Overloading the method
    void Instantiate(GameObject prefab, Vector3 spawnPoint)
    {
        Instantiate(prefab,spawnPoint);
    }

    void Instantiate(GameObject prefab, Transform parent)
    {
        Instantiate(prefab, parent);
    }
}
