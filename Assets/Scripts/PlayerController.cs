using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Animations;
using Enums;

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
    public float accelerationReduction = 1.0f;

    [Header("Arrow")]
    public float arrowCD = 1.0f;
    public float arrowCurrentCD = 0.50f;
    private int arrowCount = 5;
    public int arrowMaxCount = 5;

    [Header("Shield")]
    public GameObject shieldOBJ;
    public float shieldCD = 5.0f;
    public float shieldCurrentCD = 0;
    public float shieldMaxDuration = 5f;
    private float shieldDuration;
    private bool shieldOn;

    [Header("Slash")]
    public GameObject slashObj;
    public float slashCD = 1.0f;
    public float slashCurrentCD = 0.50f;

    public bool isSlashing = false;
    public float timerSlash = 1f;
    public GameObject knightSword;


    [Header("OTHER")]
    public GameObject spawner;
    public GameObject arrow;
    public int health = 3;
    public float laneSwapSpeed = 5f;
    private bool usedMovementLastTime = false;
    private bool isDisabled = false;

    public Animator anim_knight, anim_mage, anim_ranger;

    public bool resetPlayerStats = false;

    [Header("AUDIO")]
    public AudioClip sfx_jump;
    public AudioClip sfx_hurt, sfx_shield, sfx_arrow, sfx_sword;
    private AudioSource audioSource;

    private Vector3 startCenterPos;



    // Start is called before the first frame update
    void Start()
    {
        startCenterPos = transform.position;

        audioSource = GetComponent<AudioSource>();

        if (resetPlayerStats == false)
        {
            arrowCount = PlayerStats.arrowCount;
            health = PlayerStats.playerHealth;
            shieldMaxDuration = PlayerStats.shieldDuration;
        }

        else
        {
            arrowCount = arrowMaxCount;
            PlayerStats.shieldDuration = shieldMaxDuration;
            PlayerStats.arrowCount = arrowCount;
            PlayerStats.playerHealth = health;
        }

        shieldDuration = shieldMaxDuration;

        GameManager.Instance.GuiManager.SetupSliders(shieldCD, arrowCD, slashCD);
        GameManager.Instance.GuiManager.UpdateHealth(health);
        GameManager.Instance.GuiManager.UpdateArrows(arrowCount);

        //Hardcoded
        GameManager.Instance.player = this;
        GameManager.Instance.playerStartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisabled) return;

        Movement();
        ChangeLanes();
        Jump();

        Shield();
        Arrow();
        Slash();

        anim_mage.speed = anim_ranger.speed = currentSpeed * .5f;

        if (isSlashing == false)
            anim_knight.speed = currentSpeed * .5f;

        UpdateProgressGUI();

    }

    private void UpdateProgressGUI()
    {
        GameManager.Instance.GuiManager.UpdateSliders(shieldCD-shieldCurrentCD, arrowCD-arrowCurrentCD, slashCD-slashCurrentCD);
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
            Telemetry.slashUsed++;

            anim_knight.speed = 1f;
            anim_knight.SetTrigger("slash");
            anim_knight.speed = 1f;

            knightSword.transform.localScale = new Vector3(1.57f * 4, 1.57f * 4, 1.57f * 4);

            timerSlash = 1f;
            isSlashing = true;

            audioSource.PlayOneShot(sfx_sword);
        }

        //Cooldowns
        if (slashCurrentCD > 0)
            slashCurrentCD -= Time.deltaTime;

        if (isSlashing == true)
        {
            timerSlash -= Time.deltaTime;

            if (timerSlash <= 0)
            {
                isSlashing = false;
                knightSword.transform.localScale = new Vector3(1.57f, 1.57f, 1.57f);
            }
        }
        
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
            Telemetry.shieldUsed++;

            anim_mage.SetTrigger("shield");

            audioSource.PlayOneShot(sfx_shield);
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

            anim_ranger.SetTrigger("arrow");
            Telemetry.arrowsUsed++;
            GameManager.Instance.GuiManager.UpdateArrows(arrowCount);

            audioSource.PlayOneShot(sfx_arrow);
        }

        //Cooldowns
        if (arrowCurrentCD > 0)
            arrowCurrentCD -= Time.deltaTime;

        //if (GameManager.Instance.isCSVLogging)
        
    }

    private void OnTriggerEnter(Collider other)
    {

        //Lose Speed
        if (other.CompareTag("Obstacle"))
        {
            //if sheild but touch hole still slow down
            var script = other.GetComponent<Obstacle>();

            if (shieldOn == false)
            {
                currentSpeed -= accelerationReduction;
                script.Explode(true);
                Telemetry.hitObstacles++;


                if (health > 1)
                {
                    anim_knight.SetTrigger("hurt");
                    anim_mage.SetTrigger("hurt");
                    anim_ranger.SetTrigger("hurt");
                }

                else
                    Death();


                health--;
                GameManager.Instance.GuiManager.UpdateHealth(health);
                //audioSource.PlayOneShot(sfx_hurt);


            }

            if (currentSpeed < 0)
                currentSpeed = 0;
        }

        if (other.CompareTag("End"))
        {
            CompletedLevel();
        }
    }

    private void Jump()
    {
        if (transform.position.y <= FLOOR_HEIGHT && Input.GetButtonDown("Jump"))
        {
            //Debug.Log("JUMP!");
            //Jump
            anim_knight.SetBool("airborne", true);
            anim_mage.SetBool("airborne", true);
            anim_ranger.SetBool("airborne", true);

            yVelocity = jumpPower;

            audioSource.PlayOneShot(sfx_jump);

            Telemetry.jumpsUsed++;
        }

        //IF AIRBORNE FALLING
        if (transform.position.y > FLOOR_HEIGHT)
        {
            yVelocity -= fallSpeed * Time.deltaTime;
        }        

        //If touches ground
        if (yVelocity <= 0 && (transform.position.y <= FLOOR_HEIGHT))
        {
            yVelocity = 0;
            transform.position = new Vector3(transform.position.x, FLOOR_HEIGHT, transform.position.z);

            anim_knight.SetBool("airborne", false);
            anim_mage.SetBool("airborne", false);
            anim_ranger.SetBool("airborne", false);
        }

        Vector3 targetYVelocity = new Vector3(transform.position.x, transform.position.y + yVelocity, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetYVelocity, jumpSpeed * Time.deltaTime);
    }

    private void ChangeLanes()
    {
        //print(Input.GetAxisRaw("Horizontal"));

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

    private void Death()
    {
        GameManager.Instance.PlayDeath();

        GameManager.Instance.GuiManager.UpdateHealth(0);
        anim_knight.speed = anim_mage.speed = anim_ranger.speed = .5f;

        isDisabled = true;
        anim_knight.SetTrigger("death");
        anim_mage.SetTrigger("death");
        anim_ranger.SetTrigger("death");

        //Telemetry
        Telemetry.playerDied = true;
        Telemetry.arrowsLeft = arrowCount;
        Telemetry.remainingHealth = health;

        GameManager.Instance.GuiManager.ShowDeathScreen();

 
    }

    private void CompletedLevel()
    {
        isDisabled = true;


        //UpdatePlayer Stats
        PlayerStats.playerHealth = health;
        PlayerStats.arrowCount = arrowCount;
        GameManager.Instance.PlayFanfare();

        //Telemetry
        Telemetry.arrowsLeft = arrowCount;
        Telemetry.remainingHealth = health;

        GameManager.Instance.GuiManager.ShowCompleteScreen();
    }

    private bool isHorizontalUsed()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f)
            return true;

        return false;
    }
}
