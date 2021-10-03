using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;
    PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        playerStats = GetComponent<PlayerStats>();

    }

    // Update is called once per frame
    void Update()
    {
        //Disable input on death
        if (playerStats.CanMove)
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            player.SetDirectionalInput(directionalInput);

            if (Input.GetButtonDown("Jump"))
            {
                player.Jump();
            }
        }
        else
        {
            player.SetDirectionalInput(Vector2.zero);
        }
    }
}
