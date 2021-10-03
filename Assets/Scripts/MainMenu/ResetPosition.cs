using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    public GameObject resetObject;
    public float flowSpeed;
    public float resetToHere;
    public float resetFromHere;
    
    void FixedUpdate()
    {
        if (transform.position.x <= resetFromHere)
        {
            resetObject.transform.position += new Vector3(resetToHere, 0, 0);
        }
        else
        {
            Vector3 pos = transform.position;
            pos.x += flowSpeed * Time.deltaTime;
            transform.position = pos;
        }
    }
}
