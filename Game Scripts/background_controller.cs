using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background_controller : MonoBehaviour
{

    private Transform TRANSFORM;
    private float distance_travelled;
    [SerializeField] private float MAX_DISTANCE;
    [SerializeField] private float RETURN_POSITION_X;
    [SerializeField] private float MAX_POSITION_X;

    void Start() {
        TRANSFORM = GetComponent<Transform>();
        distance_travelled = 0;
    }

    void Update() {
        distance_travelled += 0.001f;
        TRANSFORM.position = new Vector3(TRANSFORM.position.x - 0.0008f, TRANSFORM.position.y, TRANSFORM.position.z);
        if (TRANSFORM.position.x < MAX_POSITION_X) {
            distance_travelled = 0;
            TRANSFORM.position = new Vector3(RETURN_POSITION_X, TRANSFORM.position.y, TRANSFORM.position.z);
        }
    }

}