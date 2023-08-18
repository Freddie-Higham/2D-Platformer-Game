using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerup_controller : MonoBehaviour
{

    private Transform transform;

    public float bounce_height;
    public float initial_height;
    public float move_speed;
    public float direction;

    public float height_difference_requirement;
    public GameObject this_player;
    public GameObject other_player;
    private GameObject game_object;
    private Renderer renderer;

    private Transform this_player_transform;
    private Transform other_player_transform;

    [SerializeField] private bool permanent;
 
    private bool in_view;

    void Awake()
    {
        transform = GetComponent<Transform>();
        this_player_transform = this_player.GetComponent<Transform>();
        other_player_transform = other_player.GetComponent<Transform>();
        renderer = GetComponent<Renderer>();
        game_object = GetComponent<GameObject>();

        initial_height = transform.localPosition.y;

        direction = move_speed;

        in_view = false;
        transform.localPosition = new Vector3(transform.localPosition.x - 100, transform.localPosition.y, transform.localPosition.z);

      //  gameObject.SetActive(false);
    }

    void Update()
    {

        if ((((other_player_transform.localPosition.y - height_difference_requirement) > this_player_transform.localPosition.y) || (permanent)) && !in_view) {
            in_view = true;
            transform.localPosition = new Vector3(transform.localPosition.x + 100, transform.localPosition.y, transform.localPosition.z);
        } 
        else if (((other_player_transform.localPosition.y + (height_difference_requirement / 2) < this_player_transform.localPosition.y) && (!permanent)) && in_view) {
            in_view = false;
            transform.localPosition = new Vector3(transform.localPosition.x - 100, transform.localPosition.y, transform.localPosition.z);
        }

        if ((initial_height + bounce_height) < transform.localPosition.y) {
            direction = -move_speed;
        }
        else if (initial_height > transform.localPosition.y){
            direction = move_speed;
        }

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + direction, transform.localPosition.z);
 
    }
}
