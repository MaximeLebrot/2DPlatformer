using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCollapse : MonoBehaviour
{
    public GameObject Bridge;
    public GameObject BridgeJointed;

    public void CollapseBridge()
    {
        BridgeJointed.SetActive(true);
        Bridge.SetActive(false);
    }
}
