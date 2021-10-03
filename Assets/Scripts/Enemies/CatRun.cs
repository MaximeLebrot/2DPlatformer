using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRun : MonoBehaviour
{
    public float catSpeed = 2.5f;

    void Update()
    {
        transform.Translate(new Vector3(1, 0, 0) * catSpeed * Time.deltaTime);
    }
}
