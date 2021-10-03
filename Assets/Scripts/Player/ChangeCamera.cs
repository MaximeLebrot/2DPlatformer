using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCamera : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag == "CameraTrigger")
        {
            col.transform.GetComponentInChildren<CinemachineVirtualCamera>().Priority = 100;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag == "CameraTrigger")
        {
            col.transform.GetComponentInChildren<CinemachineVirtualCamera>().Priority = 10;
        }
    }

}
