using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Animations;
using Enums;
using XInputDotNetPure;

public class PlayerController : MonoBehaviour
{




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
    private float arrowPrevCD = 0.50f;
    public int arrowCount = 5;
    public int arrowMaxCount = 5;

    [Header("Shield")]
    public GameObject shieldOBJ;
    public float shieldCD = 5.0f;
    private float shieldPrevCD = 0;
    public float shieldCurrentCD = 0;
    public float shieldMaxDuration = 5f;
    private float shieldDuration;
    private bool shieldOn;

    [Header("Slash")]
    public Slash slashObj;
    public float slashCD = 1.0f;
    public float slashCurrentCD = 0.50f;
    private float slashPrevCD = 0.50f;

    public bool isSlashing = false;
    public float timerSlash = 0.0f;
    public GameObject knightSword;

    [Header("Knocked Down")]
    private float knockoutTimer = 1.0f;
    private bool isKnockedOut = false;
    public float invincibilityMaxTimer = 1.25f;
    private bool isInvincible = false;
    private float invincibilityCurrentTimer = 0.0f;
    

    [Header("OTHER")]
    public int lanePos = 0;
    public GameObject spawner;
    public GameObject arrow;
    public int health = 3;
    public float laneSwapSpeed = 5f;
    private bool usedMovementLastTime = false;
    public bool isDisabled = false;
    public bool lockedMovement = false;

    //Perk
    private int jumpHitboxPerkCounter = 0;
    private float regenHealthMaxTimer = 12f;
    private float regenHealthTimer = 5f;

    public Animator anim_knight, anim_mage, anim_ranger;

    public bool resetPlayerStats = false;

    [Header("AUDIO")]
    public AudioClip sfx_jump, sfx_pickup;
    public AudioClip sfx_hurt, sfx_shield, sfx_arrow, sfx_sword, sfx_notifiyAbility;
    private AudioSource audioSource;

    private Vector3 startCenterPos;

    //Vibration
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;



    // Start is called before the first frame update
    void Start()
    {
        startCenterPos = transform.position;

        audioSource = GetComponent<AudioSource>();

        GameManager.Instance.player = this;
    }

    private void FixedUpdate()
    { 
        if (isKnockedOut)
            GamePad.SetVibration(playerIndex, 1.0f, 1.0f);
        else
            GamePad.SetVibration(playerIndex, 0.0f, 0.0f);
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

        UpdateProgressGUI();


        //Animation from speed
        //if (isKnockedOut)
        //anim_mage.speed = anim_ranger.speed = 1f;

        //else
        anim_mage.speed = anim_ranger.speed = currentSpeed * .5f;


        //Slashing countdown
        if (isSlashing == false)
            anim_knight.speed = currentSpeed * .5f;

        if (isKnockedOut)
        {
            knockoutTimer -= Time.deltaTime;

            if (knockoutTimer <= 0)
                isKnockedOut = false;
        }


        //PERK REGEN HEALTH
        if (PlayerStats.regenHealthIfNotHit && health < 5)
        {
            regenHealthTimer -= Time.deltaTime;
            if (regenHealthTimer <= 0)
            {
                health++;
                audioSource.PlayOneShot(sfx_pickup);
                GameManager.Instance.GuiManager.UpdateHealth(health);
                regenHealthTimer = regenHealthMaxTimer;
            }
        }

        if (isInvincible)
        {
            invincibilityCurrentTimer -= Time.deltaTime;

            if (invincibilityCurrentTimer <= 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.Slash))
            CompletedLevel();
    }

    public void SwordCdReduce()
    {
        if (yVelocity != 0 && PlayerStats.swordSizeIncrease == true)
        {
            slashCurrentCD -= 0.35f;
        }
    }

    public void UpdateProgressGUI()
    {
        GameManager.Instance.GuiManager.UpdateSliders(shieldCD - shieldCurrentCD, arrowCD - arrowCurrentCD, slashCD - slashCurrentCD);
    }

    private void Movement()
    {


        if (currentSpeed < topSpeed)
            currentSpeed += acceleration * Time.deltaTime;

        if (isKnockedOut) return;

        if (lockedMovement == false)
            transform.Translate(transform.forward * currentSpeed * Time.deltaTime);
    }

    private void Slash()
    {
        //Slash
        if (Input.GetButtonDown("Slash") && slashCurrentCD <= 0)
        {


            slashObj.Activate();

            slashCurrentCD = slashCD;
            Telemetry.slashUsed++;

            anim_knight.speed = 1f;
            anim_knight.SetTrigger("slash");
            anim_knight.speed = 1f;

            const float SWORD_SCALE_INIT = 3f;
            float swordScaling = SWORD_SCALE_INIT;

            //Perk
            if (yVelocity != 0 && PlayerStats.swordSizeIncrease == true)
                swordScaling += SWORD_SCALE_INIT * 2.5f;

            knightSword.transform.localScale = new Vector3(1.57f * swordScaling, 1.57f * swordScaling, 1.57f * swordScaling);

            timerSlash = 0.70f;
            isSlashing = true;

            audioSource.PlayOneShot(sfx_sword);
        }

        if (slashCurrentCD > 0)
        {
            slashCurrentCD -= Time.deltaTime;

            if (slashCurrentCD <= 0 && slashPrevCD != slashCurrentCD)
            {
                audioSource.PlayOneShot(sfx_notifiyAbility);
                slashPrevCD = slashCurrentCD;
            }
        }

        //If in slash mode change size!
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
        if (Input.GetButtonDown("Shield") && shieldCurrentCD <= 0 && shieldOn == false)
        {
            //print("SHIELD!");
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

        if (shieldCurrentCD > 0 && shieldOn == false)
        {
            shieldCurrentCD -= Time.deltaTime;

            if (shieldCurrentCD <= 0 && shieldPrevCD != shieldCurrentCD)
            {
                audioSource.PlayOneShot(sfx_notifiyAbility);
                shieldPrevCD = shieldCurrentCD;
            }
        }


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
        {
            arrowCurrentCD -= Time.deltaTime;

            if (arrowCurrentCD <= 0 && arrowPrevCD != arrowCurrentCD)
            {
                audioSource.PlayOneShot(sfx_notifiyAbility);
                arrowPrevCD = arrowCurrentCD;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDisabled || isInvincible) return;

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
                isKnockedOut = true;
                knockoutTimer = 0.75f;
                isInvincible = true;
                invincibilityCurrentTimer = invincibilityMaxTimer;

                //PERK
                if (PlayerStats.regenHealthIfNotHit)
                    regenHealthTimer = regenHealthMaxTimer;
                if (PlayerStats.arrowJumpPerk)
                    jumpHitboxPerkCounter = 0;

                GameManager.Instance.GuiManager.UpdateHealth(health);
                //audioSource.PlayOneShot(sfx_hurt);
            }

            if (currentSpeed < 0)
                currentSpeed = 0;
        }

        //PERK
        else if (other.CompareTag("JumpHitbox") && PlayerStats.arrowJumpPerk)
        {
            if (arrowCount < 5)
                jumpHitboxPerkCounter++;

            if (jumpHitboxPerkCounter >= 4 && arrowCount < 5)
            {
                arrowCount++;
                audioSource.PlayOneShot(sfx_pickup);
                GameManager.Instance.GuiManager.UpdateArrows(arrowCount);
                jumpHitboxPerkCounter = 0;
            }
        }

        else if (other.CompareTag("End"))
        {
            CompletedLevel();
        }

        else if (other.CompareTag("Treasure"))
        {
            arrowCount++;
            audioSource.PlayOneShot(sfx_pickup);
            GameManager.Instance.GuiManager.UpdateArrows(arrowCount);
            Destroy(other.gameObject);
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
        if (isKnockedOut) return;

        Vector3 targetLane = transform.position;


        if (Input.GetAxisRaw("Horizontal") > 0 && lanePos != 1 && usedMovementLastTime == false)
        {
            targetLane = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            lanePos++;
            usedMovementLastTime = true;
        }

        else if (Input.GetAxisRaw("Horizontal") < 0 && lanePos != -1 && usedMovementLastTime == false)
        {
            targetLane = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            lanePos--;
            usedMovementLastTime = true;
        }

        if (isHorizontalUsed() == false)
            usedMovementLastTime = false;


        //transform.position = Vector3.Lerp(transform.position, targetLane, laneSwapSpeed * Time.deltaTime);
        transform.position = targetLane;

    }

    public void DisableAnim()
    {
        anim_knight.enabled = false;
        anim_mage.enabled = false;
        anim_ranger.enabled = false;
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
