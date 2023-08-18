using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghost_controller : MonoBehaviour
{

    public float speed;
    public float initial_speed;
    public float max_speed;
    public float acceleration;

    private float player_input;
    private float GHOST_OFFSET;

    private SpriteRenderer renderer;
    private Animator animation;
    private Transform transform;

    public GameObject player;

    [SerializeField] private player_controller player_script;

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
        animation = GetComponent<Animator>();
        transform = GetComponent<Transform>();

        max_speed = 0;
        acceleration = 0;
        initial_speed = 0;
        GHOST_OFFSET = 7.3f;
    }

    public void ShiftToFox() {
        animation.SetTrigger("fox");
        transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
    }

    public void ShiftToRabbit() { 
        animation.SetTrigger("rabbit");
        transform.localScale = new Vector3(2.149f, 2.149f, 2.149f);
    }

    public void ShiftToChicken() { 
        animation.SetTrigger("chicken");
        transform.localScale = new Vector3(1.5395f, 1.607f, 1);
    }

    void Update()
    {

        speed = player_script.speed;
        max_speed = player_script.max_speed;
        acceleration = player_script.acceleration;

        if (player.tag == "player_1" && (Input.GetKey("a") || Input.GetKey("d"))) {
            player_input = Input.GetAxis("Horizontal");
        }
        else if (player.tag == "player_2" && (Input.GetKey("left") || Input.GetKey("right"))) {
            player_input = Input.GetAxis("Horizontal");
        }
        else {
            player_input = 0;
        }

        if (player_input > 0) {
            renderer.flipX = true;
        }
        else if (player_input < 0) {
            renderer.flipX = false;
        }
        
        if (player_input != 0 && speed < max_speed) {
            speed += acceleration;
            animation.SetBool("walk", true);
        }
        else if (player_input == 0) {
            if (speed > initial_speed + acceleration) {
                speed -= acceleration;
            }
            animation.SetBool("walk_faster", false);
            animation.SetBool("walk", false);
        }
        else if (speed > max_speed) {
            animation.SetBool("walk_faster", true); 
        }

        if (player.tag == "player_1") {
            transform.position = new Vector3(player.transform.position.x + GHOST_OFFSET,  player.transform.position.y, player.transform.position.z);
        }
        else if (player.tag == "player_2") {
            transform.position = new Vector3(player.transform.position.x - GHOST_OFFSET,  player.transform.position.y, player.transform.position.z);
        }
        
    }
}