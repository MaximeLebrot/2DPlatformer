using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRunTrigger : MonoBehaviour
{
    public GameObject Cat;

    public void CatTriggered()
    {
        Cat.SetActive(true);
    }
}
