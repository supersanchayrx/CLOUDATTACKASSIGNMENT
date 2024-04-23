using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]

public class USER 
{
    /*Player player;*/
    public string user_name, user_email;
    public int points;

    public USER()
    {
        /*GameObject playerObject = GameObject.Find("player");

        // Check if the GameObject with the script is found
        if (playerObject != null)
        {
            // Get the 'Player' component attached to the GameObject
            Player player = playerObject.GetComponent<Player>();

            // Assign values from the 'Player' component to the 'USER' object
            if (player != null)
            {
                points = player.points;
                user_name = player.username;
                user_email = player.useremail;
            }

        }
        else
        {
            Debug.Log("cannotFindPlayer");
        }*/

        user_name = Player.username;
        user_email = Player.useremail;
        points = Player.points;
    }
}
