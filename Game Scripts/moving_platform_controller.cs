using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class moving_platform_controller : MonoBehaviour 
{

    private Transform transform;
    private Rigidbody2D body;

    public float left_distance;
    public float right_distance;
    public float speed;
    public float beginning_x;
    public float direction;

    void Awake()
    {

        transform = GetComponent<Transform>();
        body = GetComponent<Rigidbody2D>();

        //Debug.Log( body.transform.localPosition);
        beginning_x = body.transform.localPosition.x;

        direction = -1;

    }

    void Update()
    {

        //Debug.Log( body.transform.localPosition);

        if (direction == -1 && ( body.transform.localPosition.x + left_distance) < beginning_x) {
           // Debug.Log($"(direction == -1 && ({ body.transform.localPosition.x} + {left_distance}) > {beginning_x})");
            direction = 1;
        }
        else if (direction == 1 && ( body.transform.localPosition.x - right_distance) > beginning_x) {
           // Debug.Log($"(direction == 1 && ({ body.transform.localPosition.x} - {left_distance}) > {beginning_x})");
            direction = -1;
        }

        body.velocity = new Vector2(speed * direction, 0);
        
    }
}
