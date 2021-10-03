using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{

    public GameObject target;
    public float followSpeed;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + offset , followSpeed * Vector3.Distance(transform.position,target.transform.position) * Time.deltaTime);
    }
}
