using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallMovement : MonoBehaviour
{
    private const int V = 3;
    //public Camera main_camera;
    public GameObject camera_parent;
    Rigidbody rb;
    Boolean gravity_inverted = false;
    Boolean onGround = true;

    public GameObject lowerPlane;
    public GameObject upperPlane;

    float timer_;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        this.GetComponent<Rigidbody>().freezeRotation = true;
        timer_ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer_ += Time.deltaTime;

        //forward
        float speed = Time.deltaTime * 3f;

        transform.Translate(Vector3.back * speed);
        camera_parent.transform.Translate(Vector3.forward * speed);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (gravity_inverted)
            {
                if (transform.position.x >= -10)
                    transform.Translate(Vector2.right * 10f * Time.deltaTime);
            }
            else
            {
                if (transform.position.x <= 10)
                    transform.Translate(Vector2.left * 10f * Time.deltaTime);   
            }   
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (gravity_inverted)
            {
                if (transform.position.x <= 10)
                    transform.Translate(Vector2.left * 10f * Time.deltaTime);
            }
            else
            {
                if (transform.position.x >= -10)
                    transform.Translate(Vector2.right * 10f * Time.deltaTime);     
            }
            
        }

        //jump
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (onGround)
            {
                if (gravity_inverted)
                {
                    rb.AddForce(Vector3.down * 100f * V);
                    onGround = false;
                }

                else
                {
                    rb.AddForce(Vector3.up * 100f * V);
                    onGround = false;
                }
            }
        }

        //invert gravity
        if (Input.GetKeyDown(KeyCode.UpArrow) && timer_ > 1f)
        {
            timer_ = 0; 
            Boolean temp = false;

            if(!gravity_inverted)
                temp = true;

            else
                temp = false;

            gravity_inverted = temp;

            if (gravity_inverted)
            {
                Physics.gravity = -1 * Physics.gravity;

                camera_parent.transform.Rotate(0, 0, 180);
            }

            else
            {
                Physics.gravity = -1 * Physics.gravity;
                camera_parent.transform.Rotate(0, 0, 180);
            }
        }

        //reset game
        float timer = 0;
        timer += Time.deltaTime;


        //sphere falling
        if (transform.position.y < 0 || transform.position.y > 5) 
        {
            Debug.Log(" sphere fell");

            if(gravity_inverted)
                Physics.gravity = -1 * Physics.gravity;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //sphere hits obstacles
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "lowerCube(Clone)" || other.gameObject.name == "upCube(Clone)"
            || other.gameObject.name == "lowerCube" || other.gameObject.name == "upCube")
        {
            //Debug.Log(" sphere hit a cube");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            if (gravity_inverted)
                Physics.gravity = -1 * Physics.gravity;
        }

        if (other.gameObject.name.Contains("Plane"))
            onGround = true;
    }

}
