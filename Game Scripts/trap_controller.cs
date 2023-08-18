using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap_controller : MonoBehaviour {

    private bool triggered;
    private float transparency;

    private SpriteRenderer RENDERER;
    private Transform TRANSFORM;

    void Start() {
        triggered = false;
        RENDERER = GetComponent<SpriteRenderer>();
        TRANSFORM = GetComponent<Transform>();
        transparency = 1f;
    }

    void Update() {
        if (triggered) {
            transparency -= 0.007f;
            RENDERER.color = new Color(1f, 1f, 1f, transparency); 
            TRANSFORM.position = new Vector3(TRANSFORM.position.x, TRANSFORM.position.y - 0.015f, TRANSFORM.position.z);
        }
        if (transparency == 0) {

        }
    }

    public bool trigger() {
        if (!triggered){
            triggered = true;
            return true;
        }
        else {
            return false;
        }
    }

}