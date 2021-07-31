using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    private int position = 0;

    //jUMP
    private const float FLOOR_HEIGHT = 4.465f;
    public float yVelocity = 0;
    public float jumpPower = 12;
    public float jumpSpeed = 1f;
    public float fallSpeed = 9.81f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Forward"))
        //{
            transform.Translate(transform.forward * speed * Time.deltaTime);
        //}

        ChangeLanes();
        Jump();


    }

    private void Jump()
    {
        if (transform.position.y <= FLOOR_HEIGHT && Input.GetButtonDown("Jump"))
        {
            Debug.Log("JUMP!");
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
        if (Input.GetButtonDown("Horizontal"))
        {
            if (Input.GetAxisRaw("Horizontal") > 0 && position != 1)
            {
                transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                position++;
            }

            else if (Input.GetAxisRaw("Horizontal") < 0 && position != -1)
            {
                transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                position--;
            }
        }

    }
}
