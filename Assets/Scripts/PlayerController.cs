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

    [Header("Arrow")]
    public float arrowCD = 1.0f;
    private float arrowCurrentCD = 0.50f;
    private int arrowCount = 5;
    public int arrowMaxCount = 5;

    [Header("Shield")]
    public GameObject shieldOBJ;
    public float shieldCD = 5.0f;
    private float shieldCurrentCD = 0;
    public float shieldMaxDuration = 5f;
    private float shieldDuration;
    private bool shieldOn;

    [Header("Slash")]
    public GameObject slashObj;
    public float slashCD = 1.0f;
    private float slashCurrentCD = 0.50f;

    [Header("OTHER")]
    public GameObject spawner;
    public GameObject arrow;
    public float laneSwapSpeed = 5f;
    private bool usedMovementLastTime = false;



    // Start is called before the first frame update
    void Start()
    {
        arrowCount = arrowMaxCount;
        shieldDuration = shieldMaxDuration;
    }

    // Update is called once per frame
    void Update()
    {

        Movement();
        ChangeLanes();
        Jump();

        Shield();
        Arrow();
        Slash();

    }

    private void Movement()
    {
        if (currentSpeed < topSpeed)
            currentSpeed += acceleration * Time.deltaTime;

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime);
    }

    private void Slash()
    {
        //Slash
        if (Input.GetButtonDown("Slash") && slashCurrentCD <= 0)
        {
            slashObj.SetActive(true);

            slashCurrentCD = slashCD;
        }

        //Cooldowns
        if (slashCurrentCD > 0)
            slashCurrentCD -= Time.deltaTime;
    }

    private void Shield()
    {
        //Shield
        if (Input.GetButtonDown("Shield") && shieldCurrentCD <= 0)
        {
            print("SHIELD!");
            //Show shield
            shieldOBJ.SetActive(true);
            shieldOn = true;
            shieldDuration = shieldMaxDuration;
        }

        if (shieldOn)
        {
            shieldDuration -= Time.deltaTime;

            if (shieldDuration <= 0f)
            {
                shieldOn = false;
                shieldOBJ.SetActive(false);
                shieldCurrentCD = shieldCD;
            }
        }

        //Cooldowns
        if (shieldOn == false && shieldCurrentCD > 0)
            shieldCurrentCD -= Time.deltaTime;
    }

    private void Arrow()
    {

        //Arrow
        if (Input.GetButtonDown("Arrow") && arrowCurrentCD <= 0 && arrowCount > 0)
        {
            //Instastiate Arrow infront
            Instantiate(arrow, spawner.transform.position, spawner.transform.rotation);
            arrowCurrentCD = arrowCD;
            arrowCount--;
        }

        //Cooldowns
        if (arrowCurrentCD > 0)
            arrowCurrentCD -= Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (shieldOn) return;

        //Lose Speed
        if (other.CompareTag("Obstacle"))
        {
            //print("HIT!");
            currentSpeed -= accelerationReduction;

            if (currentSpeed < 0)
                currentSpeed = 0;
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
        print(Input.GetAxisRaw("Horizontal"));

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
