using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trophy_controller : MonoBehaviour
{

    private GameObject camera_1;
    private GameObject camera_2;
    private GameObject player_1;
    private GameObject player_2;

    [SerializeField] private AudioSource OVERTAKE_SOUND;
    [SerializeField] private AudioSource WIND_SOUND;
    [SerializeField] private AudioSource START_SOUND;
    [SerializeField] private AudioSource MUSIC;

    //private player_movement player_script;

    private string current_camera;
    private bool started;
    private bool first_overtake;

    private Transform transform;

    private float timer;

    void Start()
    {

        camera_1 = GameObject.FindGameObjectWithTag("camera_1");
        camera_2 = GameObject.FindGameObjectWithTag("camera_2");
        player_1 = GameObject.FindGameObjectWithTag("player_1");
        player_2 = GameObject.FindGameObjectWithTag("player_2");

        transform = GetComponent<Transform>();
        
        timer = -11.5f;
        //timer = 0;

        current_camera = "";

        started = false;
        first_overtake = false;

        //player_script = player_1.GetComponent<player_controller>();
    }

    void Update()
    {

        timer += Time.deltaTime;

        if (timer > 1.5f) {

            timer = 0;

            if (player_1.GetComponent<Transform>().position.y > player_2.GetComponent<Transform>().position.y) {

                if (current_camera != "camera_1" && first_overtake) {
                    OVERTAKE_SOUND.panStereo = -0.5f;
                    OVERTAKE_SOUND.Play();
                }
                
                current_camera = "camera_1";
                transform.position = new Vector3(-5.67f, camera_1.GetComponent<Transform>().position.y + 3.88f, transform.position.z);
            }
            else {

                if (current_camera != "camera_2" && first_overtake) {
                    OVERTAKE_SOUND.panStereo = 0.5f;
                    OVERTAKE_SOUND.Play();
                }

                current_camera = "camera_2";
                transform.position = new Vector3(7.73f, camera_2.GetComponent<Transform>().position.y + 3.88f, transform.position.z);
            }

            first_overtake = true;
        }

        else if (timer > 0 && !MUSIC.isPlaying) {
            MUSIC.Play();
        }

        else if (timer > -3.5f && !started) {
            started = true;
            START_SOUND.Play();
        }

        else {
            if (current_camera == "camera_1") {
                transform.position = new Vector3(-5.67f, camera_1.GetComponent<Transform>().position.y + 3.88f, transform.position.z);
            }
            else if (current_camera == "camera_2"){
                transform.position = new Vector3(7.73f, camera_2.GetComponent<Transform>().position.y + 3.88f, transform.position.z);
            }
            else {
                transform.position = new Vector3(100f, camera_1.GetComponent<Transform>().position.y + 3.88f, transform.position.z);
            }

         }
        
    }

}

