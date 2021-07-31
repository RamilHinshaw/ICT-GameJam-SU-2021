using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed = 1f;
    private int position = 0;

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


    }

    void ChangeLanes()
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
