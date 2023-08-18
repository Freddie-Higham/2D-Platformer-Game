using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class farmer_controller : MonoBehaviour
{
    [SerializeField] private float LEFT_DISTANCE;
    [SerializeField] private float RIGHT_DISTANCE;
    [SerializeField] private float SPEED;

    [SerializeField] private bool agro;
    [SerializeField] private bool fear;

    private Rigidbody2D BODY;
    private SpriteRenderer RENDERER;
    private BoxCollider2D BOX_COLLIDER;

    [SerializeField] private LayerMask PLAYER_LAYER_MASK;

    private RaycastHit2D raycastHit;

    [SerializeField] private player_controller player_script;

    private Animator ANIMATION;

    private int direction;

    private float BEGINNING_X;

    void Start()
    {
        BODY = GetComponent<Rigidbody2D>();
        RENDERER = GetComponent<SpriteRenderer>();
        BOX_COLLIDER = GetComponent<BoxCollider2D>();
        ANIMATION = GetComponent<Animator>();

        direction = -1;

        agro = false;

        BEGINNING_X = BODY.transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D raycast_hit_front = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 3f, PLAYER_LAYER_MASK);
        RaycastHit2D raycast_hit_back = Physics2D.Raycast(transform.position, new Vector2(-direction, 0), 3f, PLAYER_LAYER_MASK);

        Debug.DrawRay(transform.position, new Vector2(direction, 0) * 3, Color.green, 0.01f);
        Debug.DrawRay(transform.position, new Vector2(-direction, 0) * 3, Color.red, 0.01f);

        if (fear || agro) {
            ANIMATION.SetBool("walking_faster", true);
        }
        else {
            ANIMATION.SetBool("walking_faster", false);
        }

        if (raycast_hit_front.collider != null) {
            Debug.Log(raycast_hit_front.collider.gameObject);
            if (player_script.fox && !fear) {
                fear = true;
                SPEED *= 1.3f;
               // Debug.Log("Fear speed buff.");
                if (direction == 1) {
                    direction = -1;
                    RENDERER.flipX = true;
                }
                else {
                    direction = 1;
                    RENDERER.flipX = false;
                }
            }
            if (!player_script.fox && !agro){
               // Debug.Log("Agro speed buff.");
                agro = true;
                SPEED *= 1.5f;
            }
            else {
            }
        }
        else if (agro) {
            //Debug.Log("Agro is true and Collider is null.");
            agro = false;
            SPEED /= 1.5f;
        }
        else if (raycast_hit_back.collider == null && fear) {
            //Debug.Log("Fear speed reverted");
            fear = false;
            SPEED /= 1.3f;
        }

        if (!fear) {
            if (direction == -1 && (BODY.transform.localPosition.x + LEFT_DISTANCE) < BEGINNING_X) {
              //  Debug.Log("flip normal");
                RENDERER.flipX = false;
                direction = 1;
            }
            else if (direction == 1 && (BODY.transform.localPosition.x - RIGHT_DISTANCE) > BEGINNING_X) {
              //  Debug.Log("flip normal");
                RENDERER.flipX = true;
                direction = -1;
            }
        }

        BODY.velocity = new Vector2(direction * SPEED, BODY.velocity.y);
    }
}