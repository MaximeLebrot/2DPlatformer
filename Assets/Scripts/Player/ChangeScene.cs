using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public int sceneIndex;
    public bool isEnabled;
    public bool inRange;
    //Teleporting to another portal instead of changing scene
    public bool justTeleport;
    public GameObject teleportTo;

    GameObject player;

     void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Update()
    {
        if (Input.GetButtonDown("Enter") && isEnabled && inRange)
        {
            if (!justTeleport)
            {
                LoadScene();
            }
            else
            {
                Teleport();
            }
        }
    }
    public void Teleport()
    {
        player.GetComponent<PlayerStats>().FadeInScreen();
        player.transform.position = teleportTo.transform.position;
        player.GetComponent<Player>().ResetVelocity();
        
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
