using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private int position = 0;

    //jUMP
    [Header("Jump")]
    public float yVelocity = 0;
    public float jumpPower = 12;
    public float jumpSpeed = 1f;
    public float fallSpeed = 9.81f;
    private const float FLOOR_HEIGHT = 4.465f;


    //Acceleration
    [Header("Acceleration")]
    public float currentSpeed = 0;
    public float topSpeed = 3f;
    public float acceleration = 0.5f;
    public float accelerationReduction = 0.33f;

    [Header("OTHER")]
    public GameObject spawner;
    public GameObject arrow;
    public float laneSwapSpeed = 5f;
    private bool usedMovementLastTime = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Movement();
        ChangeLanes();
        Jump();

        Abilities();

    }

    private void Movement()
    {
        if (currentSpeed < topSpeed)
            currentSpeed += acceleration * Time.deltaTime;

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime);
    }

    private void Abilities()
    {
        //Shield

        //Slash

        //Arrow
        if (Input.GetButtonDown("Arrow"))
        {
            //Instastiate Arrow infront
            Instantiate(arrow, spawner.transform.position, spawner.transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Lose Speed
        if (other.CompareTag("Obstacle"))
        {
            //print("HIT!");
            currentSpeed -= currentSpeed * accelerationReduction;
        }
    }

    private void Jump()
    {
        if (transform.position.y <= FLOOR_HEIGHT && Input.GetButtonDown("Jump"))
        {
            //Debug.Log("JUMP!");
            //Jump
            yVelocity = jumpPower;
        }

        //IF AIRBORNE FALL
        if (transform.position.y > FLOOR_HEIGHT)
        {
            yVelocity -= fallSpeed * Time.deltaTime;
        }        

        if (yVelocity <= 0 && (transform.position.y <= FLOOR_HEIGHT))
        {
            yVelocity = 0;
        }

        Vector3 targetYVelocity = new Vector3(transform.position.x, transform.position.y + yVelocity, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetYVelocity, jumpSpeed * Time.deltaTime);
    }

    private void ChangeLanes()
    {
        Vector3 targetLane = transform.position;


        if (Input.GetAxisRaw("Horizontal") > 0 && position != 1 && usedMovementLastTime == false)
        {
            targetLane = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            position++;
            usedMovementLastTime = true;
        }

        else if (Input.GetAxisRaw("Horizontal") < 0 && position != -1 && usedMovementLastTime == false)
        {
            targetLane = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            position--;
            usedMovementLastTime = true;
        }

        if (isHorizontalUsed() == false)
            usedMovementLastTime = false;


        //transform.position = Vector3.Lerp(transform.position, targetLane, laneSwapSpeed * Time.deltaTime);
        transform.position = targetLane;

    }

    private bool isHorizontalUsed()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f)
            return true;

        return false;
    }
}
