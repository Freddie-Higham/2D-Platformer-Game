using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

/* 
This class manages the movement of player 1 and 2. It also manages their collisions with object such
as powerups and farmers.
*/
public class player_controller : MonoBehaviour {

///Movement attributes (changes based on shapeshft) 
//Variable
public float speed;
public float initial_speed;
public float max_speed;
public float acceleration;
public float jump_speed;
//Constant
public float INITIAL_GRAVITY;
public float CHICKEN_FALLING_GRAVITY;
public float FALLING_GRAVITY;

//Movement update variables
private float player_input;
[SerializeField] private float new_velocity_x;
[SerializeField] private float new_velocity_y;

//countdown variables
private float countdown_timer;
private int countdown_value;
private bool counting_down;

//other variables
[SerializeField] private bool grounded;
private bool chicken;
public bool fox;
private bool fox_speed_boost;
private float fox_speed_boost_timer;
public bool end;
private bool fox_boost_blinking;
private float last_fox_boost_blink_change;
private float fox_boost_blink_change_freq;
private Vector3 checkpoint_pos;
private Vector3 highest_possible_checkpoint;

//other constants
private float FOX_SPEED_BOOST_DURATION;
[SerializeField] private LayerMask PLATFORM_LAYER_MASK;
public int PLAYER_NUM;

//Sounds
[SerializeField] private AudioSource JUMP_SOUND;
[SerializeField] private AudioSource TRAP_SOUND;
[SerializeField] private AudioSource POWERUP_SOUND;
[SerializeField] private AudioSource DIE_SOUND;
[SerializeField] private AudioSource WIN_SOUND;
[SerializeField] private AudioSource FARMER_POWERUP_SOUND;

//Player components
private Rigidbody2D BODY;
private SpriteRenderer RENDERER;
private Animator ANIMATION;
private BoxCollider2D PLAYER_BOX;
private Transform TRANSFORM;

//Other components
private Transform other_player_transform;
private Transform powerup_transform;
public TextMeshProUGUI countdown_text;

//Other game objects
private GameObject[] checkpoints;
private GameObject[] powerups;
private GameObject[] finish_flags;

//Other scripts
[SerializeField] private player_controller other_player_script;
[SerializeField] private ghost_controller ghost_script;

//Start() Called once when object initialised.
//Used to initialise all the variables.
private void Start() {

    //Player components
    BODY = GetComponent<Rigidbody2D>();
    RENDERER = GetComponent<SpriteRenderer>();
    ANIMATION = GetComponent<Animator>();
    PLAYER_BOX = GetComponent<BoxCollider2D>();
    TRANSFORM = GetComponent<Transform>();

    //Other game objects
    finish_flags = GameObject.FindGameObjectsWithTag("finish_flag");
    if (PLAYER_NUM == 1) {
        checkpoints = GameObject.FindGameObjectsWithTag("checkpoint_player_1");
        powerups = GameObject.FindGameObjectsWithTag("powerup_player_1");
    }
    else if (PLAYER_NUM == 2){
        checkpoints = GameObject.FindGameObjectsWithTag("checkpoint_player_2");
        powerups = GameObject.FindGameObjectsWithTag("powerup_player_2");
    }

    //Countdown variables
    counting_down = true;
    countdown_value = 0;
    countdown_text.text = $"";

    //Other variables
    grounded = true; 
    fox = false;
    fox_speed_boost = false;
    fox_speed_boost_timer = 0;
    end = false;
    fox_boost_blinking = false;
    last_fox_boost_blink_change = 0;
    countdown_timer = -1;f

    //Other constants
    CHICKEN_FALLING_GRAVITY = 0.3f;
    FALLING_GRAVITY = 1.3f;
    INITIAL_GRAVITY = 1f;
    FOX_SPEED_BOOST_DURATION = 4;

    //Stops unwanted rotation caused by collisions.
    BODY.freezeRotation = true;

    //Set shapeshift to chicken
    ShiftToChicken();

}

private void ShiftToChicken() {
    ANIMATION.SetTrigger("chicken");
    TRANSFORM.localScale = new Vector3(1.5395f, 1.607f, 1);
    PLAYER_BOX.size = new Vector2(0.45f, 0.41f);

    chicken = true;
    fox = false;
    max_speed = 4.5f;
    acceleration = 0.008f;
    initial_speed = 2.3f;
    jump_speed = 5.5f;

    speed /= 1.2f;
    if (speed < initial_speed) {
        speed = initial_speed;
    }
    ghost_script.ShiftToChicken();
}

private void ShiftToRabbit() {
    ANIMATION.SetTrigger("rabbit");
    TRANSFORM.localScale = new Vector3(2.149f, 2.149f, 2.149f);
    PLAYER_BOX.size = new Vector2(0.28f, 0.22f);

    chicken = false;
    fox = false;
    max_speed = 2.7f;
    acceleration = 0.003f;
    initial_speed = 1.35f;
    jump_speed = 6.8f;

    speed /= 1.4f;
    if (speed < initial_speed) {
        speed = initial_speed;
    }
    ghost_script.ShiftToRabbit();
}

private void ShiftToFox() {
    ANIMATION.SetTrigger("fox");
    TRANSFORM.localScale = new Vector3(2.35f, 2.35f, 2.35f);
    PLAYER_BOX.size = new Vector2(0.40f, 0.27f);

    chicken = false;
    fox = true;
    max_speed = 3.5f;
    acceleration = 0.0035f;
    initial_speed = 1.8f;
    jump_speed = 6f;

    speed /= 1.3f;
    if (speed < initial_speed) {
        speed = initial_speed;
    } 
    ghost_script.ShiftToFox();
}

private void ApplyFoxSpeedBoost() {
    fox_speed_boost = true;
    max_speed = 4.8f;
    acceleration = 0.0056f;
    initial_speed = 3.5f;
    jump_speed = 6.5f;
    Debug.Log("new color");
    RENDERER.color = new Color(1f, 1f, 0.7272727f, 0.6392157f); 
    TRANSFORM.localScale = new Vector3(2.4f, 2.4f, 2.4f);
    PLAYER_BOX.size = new Vector2(0.45f, 0.30f);
    Debug.Log(RENDERER.color);
    last_fox_boost_blink_change = 0;
    fox_boost_blinking = false;
    fox_boost_blink_change_freq = 0.35f;
}

//Increment the countdown timer and display the countdown text at the beginning of race.
private void Countdown() {

    countdown_timer += Time.deltaTime;

    if (countdown_timer < 10) {
        if (countdown_timer >= countdown_value + 1) {
            countdown_value += 1;
            if (countdown_value >= 7) {countdown_text.text = $"{10 - countdown_value}";}
            else {countdown_text.text = "";}
        }
    }
    else {
        counting_down = false;
    }
}

private void KeyboardInputChecks() {

    //Shapeshift button input control.
    if ((Input.GetKey("3") && PLAYER_NUM == 1) || (Input.GetKey("0") && PLAYER_NUM == 2)) {
        if (!fox) {
            ShiftToFox();
        }
    }else if ((Input.GetKey("2") && PLAYER_NUM == 1) || (Input.GetKey("9") && PLAYER_NUM == 2)) {
        if ((!fox && chicken) || (fox && !chicken)) {
            ShiftToRabbit();
        }
    }else if ((Input.GetKey("1") && PLAYER_NUM == 1) || (Input.GetKey("8") && PLAYER_NUM == 2)) {
        if (!chicken) {
            ShiftToChicken();
        }
    }

    //Detects left or right input.
    if ((PLAYER_NUM == 1 && Input.GetKey("a")) || (PLAYER_NUM == 2 && Input.GetKey("left"))) {
        player_input = -1;
        RENDERER.flipX = false;
    } else if ((PLAYER_NUM == 1 && Input.GetKey("d")) || (PLAYER_NUM == 2 && Input.GetKey("right"))) {
        player_input = 1;
        RENDERER.flipX = true;
    } else {
        player_input = 0;
    }

    //Apply jump velocity when grounded and input provided. 
    if ((PLAYER_NUM == 1 && Input.GetKey("w") || PLAYER_NUM == 2 && Input.GetKey("up")) && grounded)
    {
        JUMP_SOUND.Play();
        new_velocity_y = jump_speed;
       // grounded = false;
    } 

    //Resposible for making the jump height higher the longer you hold.
    if (BODY.velocity.y > 0 && ((Input.GetKeyUp("w") && PLAYER_NUM == 1) || (Input.GetKeyUp("up") && PLAYER_NUM == 2))) {
        new_velocity_y /= 2;
    }

}

//Manages the fox-eating-farmer speed boost while active.
private void ManageFoxSpeedBoost() {

    if (fox){
        fox_speed_boost_timer += Time.deltaTime;

        if (fox_speed_boost_timer - fox_boost_blink_change_freq > last_fox_boost_blink_change) {
            last_fox_boost_blink_change = fox_speed_boost_timer;
            fox_boost_blink_change_freq -= 0.015f;
            if (fox_boost_blinking) {
                RENDERER.color = new Color(1f, 1f, 0.7272727f, 0.7392157f);
                fox_boost_blinking = false;
            }else {
                RENDERER.color = new Color(1f, 1f, 1f, 1f);
                fox_boost_blinking = true;
            }
        }

        if (fox_speed_boost_timer > FOX_SPEED_BOOST_DURATION) {
            fox_speed_boost_timer = 0;
            fox_speed_boost = false;
            Debug.Log("color reverted");
            RENDERER.color = new Color(1f, 1f, 1f, 255f);
            ShiftToFox();
        }
    }else{
        fox_speed_boost_timer = 0;
        fox_speed_boost = false;
        Debug.Log("color reverted");
        RENDERER.color = new Color(1f, 1f, 1f, 255f);
    }

}

private void UpdateSpeed() {

    //Accelerate until max speed reached.
    if (player_input != 0 && speed < max_speed) {
        speed += acceleration;
        ANIMATION.SetBool("walk", true);
    //Decelerate whilst not moving.
    } else if (player_input == 0) {
        if (speed > initial_speed + acceleration) {
            speed -= acceleration;
        }
        ANIMATION.SetBool("walk_faster", false);
        ANIMATION.SetBool("walk", false);
    } 

}

private void UpdateVelocity() {

    //Change x velocity based on input.
    new_velocity_x += speed * player_input;

    //Check what's immediately benneath.
    RaycastHit2D raycastHit = Physics2D.BoxCast(PLAYER_BOX.bounds.center, PLAYER_BOX.bounds.size, 0f, Vector2.down, 0.1f, PLATFORM_LAYER_MASK);
    //Move alongside a moving platform whilst on it and not moving yourself.
    if (raycastHit.collider != null)
    {   
        grounded = true;
        if (raycastHit.transform.tag == "moving_platform" && Math.Abs(BODY.velocity.x) <= Math.Abs(raycastHit.rigidbody.velocity.x) && player_input == 0) {
            new_velocity_x = raycastHit.rigidbody.velocity.x;
        }
    }

    if (new_velocity_y < 0) {
        ANIMATION.SetBool("falling", true);
        ANIMATION.SetBool("jumping", false);
        grounded = false;
    } 
    else if (new_velocity_y > 0) {
        ANIMATION.SetBool("jumping", true);
        ANIMATION.SetBool("falling", false);
        grounded = false;
    }
    else {
        ANIMATION.SetBool("jumping", false);
        ANIMATION.SetBool("falling", false);
    }

    SetXVelocity(new_velocity_x);
    SetYVelocity(new_velocity_y);

}

private void UpdateGravity() {

    //Make gravity really low when the chicken is falling.
    if (BODY.velocity.y < 0 && chicken) {
        BODY.gravityScale = CHICKEN_FALLING_GRAVITY;
    //Increase gravity just before you're about to fall.
    } else if (BODY.velocity.y < max_speed / 3 && chicken == false) {
        BODY.gravityScale = FALLING_GRAVITY;
    } else{
        BODY.gravityScale = INITIAL_GRAVITY;
    }

}    

private void UpdateAnimations() {

    //Start the faster movement animation if max speed is reached.
    if (speed >= max_speed) {
        ANIMATION.SetBool("walk_faster", true); 
    }

    //Syncronise the Animation boolean with the script boolean.
    ANIMATION.SetBool("grounded", grounded);

    ANIMATION.SetBool("fox_boost", fox_speed_boost); 

}

private void SetXVelocity(float x) {
    BODY.velocity = new Vector2(x, BODY.velocity.y);
}

private void SetYVelocity(float y) {
    BODY.velocity = new Vector2(BODY.velocity.x, y);
}


// Update is called once per frame
void Update()
{

    //For the beginning when there's a 10 second countdown.
    if (counting_down) {
        Countdown();
    } 

    //If countdown has finished do this until race is over.
   else if (!end) {
        new_velocity_x = 0;
        new_velocity_y = BODY.velocity.y;

        //For one second after the countdown, display the text "Go!".
        if (countdown_timer < 12) {
            countdown_timer += Time.deltaTime;
            countdown_text.text = "Go!";
        }else {
            countdown_text.text = "";
        }

        KeyboardInputChecks();

        if (fox_speed_boost) {
            ManageFoxSpeedBoost();
        }

        UpdateSpeed();
        UpdateVelocity();

        UpdateAnimations();

        UpdateGravity();
    }

}

private void OnCollisionEnter2D(Collision2D collision){

    //Check what's immediately benneath.
    RaycastHit2D raycastHit = Physics2D.BoxCast(PLAYER_BOX.bounds.center, PLAYER_BOX.bounds.size, 0f, Vector2.down, 0.1f, PLATFORM_LAYER_MASK);

    //If the ray has collided.
    if (raycastHit.collider != null){
        //If the platform below is a "trap" platform, make it dissapear.
        if (collision.gameObject.tag == "trap") {
            //Make the platform slightly transparent.
            if (collision.gameObject.GetComponent<trap_controller>().trigger()) {
                TRAP_SOUND.Play();
            };
          //  collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            //Destroy the platform.
            Destroy(collision.gameObject, 0.42f);
        }
        //Otherwise you are on a normal platform so you are grounded.
        else {
            grounded = true;
        }
    }

    //Colliding with farmer.
    if (collision.gameObject.tag == "farmer") {
        //If not a fox, you will die and respawn.
        if (fox != true) {
            highest_possible_checkpoint = new Vector3(0, -1000, 0);
            DIE_SOUND.Play();
            //Find closest checkpoint.
            foreach (GameObject game_object in checkpoints) {
                checkpoint_pos = game_object.GetComponent<Transform>().transform.position;
                if (TRANSFORM.position.y > checkpoint_pos.y && checkpoint_pos.y > highest_possible_checkpoint.y) {
                    highest_possible_checkpoint = checkpoint_pos;
                }
            } 
            //Teleport to nearest checkpoint.
            TRANSFORM.position = highest_possible_checkpoint;
        //You're a fox, so kill the farmer
        }else {
            FARMER_POWERUP_SOUND.Play();
            collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            Destroy(collision.gameObject, 0.05f);
            ApplyFoxSpeedBoost();
        }

    //Collecting powerup.
    }else if (collision.gameObject.tag == "shapeshift_powerup") {
        collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        Destroy(collision.gameObject, 0.01f);

        POWERUP_SOUND.Play();
        
        //Swap the other player's form.
        if (other_player_script.chicken == true) {
            other_player_script.ShiftToRabbit();
        }else if (other_player_script.fox == true) {
            other_player_script.ShiftToChicken();
        }else{
            other_player_script.ShiftToFox();
        }
    
    }else if (collision.gameObject.tag == "finish_flag") {
        End();
    }
}

private void End(){
    WIN_SOUND.Play();
    countdown_text.text = $"Player {PLAYER_NUM} wins!";
    other_player_script.countdown_text.text = $"Player {PLAYER_NUM} wins!";
    end = true;
    other_player_script.end = true;
}

}
