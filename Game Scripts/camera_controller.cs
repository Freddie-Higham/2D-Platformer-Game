using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Manages the camera movement in relation to the player. 
Also performs a tour of the level at the beginning. 
*/
public class camera_controller : MonoBehaviour
{
    public float last_camera_movement_y;
    private bool beginning;

    private Vector3 player_position;

    public float OFFSET;
    public float OFFSET_SMOOTHING;
    public float DISTANCE_TO_MOVE;
    public Vector3 CAMERA_STARTING_POSITION;
    
    public GameObject player;

    void Start()
    {
        last_camera_movement_y = player.transform.position.y;
        CAMERA_STARTING_POSITION = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        player_position = new Vector3(transform.position.x, player.transform.position.y + OFFSET, transform.position.z);

        transform.position = new Vector3(transform.position.x, 40f, transform.position.z);
        //transform.position = CAMERA_STARTING_POSITION;

        beginning = false;
    }

    void Update()
    {
        //Do the initial tour.
        if (beginning) {
            player_position = new Vector3(player_position.x, player_position.y + 0.2f, player_position.z);
            transform.position = Vector3.Lerp(transform.position, player_position, OFFSET_SMOOTHING * Time.deltaTime);
            player_position = new Vector3(player_position.x, player_position.y - 0.2f, player_position.z);
        //Once that has done, do the normal camera controlling.
        }else{

            //If the player has moved upwards the amount needed to justify moving the camera up.
            if (player.transform.position.y > last_camera_movement_y + DISTANCE_TO_MOVE) {
                player_position = new Vector3(transform.position.x, player.transform.position.y + OFFSET, transform.position.z);
                last_camera_movement_y = player.transform.position.y;
            //Same but checking if the camera needs moving downwards.
            }else if (player.transform.position.y < last_camera_movement_y - DISTANCE_TO_MOVE) {
                player_position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
                last_camera_movement_y = player.transform.position.y;
            }else {
                player_position = new Vector3(player_position.x, player_position.y, player_position.z);
            }

            if (player_position.y < CAMERA_STARTING_POSITION.y) {
                player_position.y = CAMERA_STARTING_POSITION.y;
            }

            transform.position = Vector3.Lerp(transform.position, player_position, OFFSET_SMOOTHING * Time.deltaTime);
        }

        //Check whether beginning or not
        if (transform.position.y < CAMERA_STARTING_POSITION.y) {
            beginning = false;
            transform.position = new Vector3(transform.position.x, CAMERA_STARTING_POSITION.y, transform.position.z);
        }
    }
}
