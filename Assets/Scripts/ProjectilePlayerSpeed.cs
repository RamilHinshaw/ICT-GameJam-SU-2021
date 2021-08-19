using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayerSpeed : MonoBehaviour
{
    public float lifeTime = 10f;
    public float speed = 1f;
    public float speedModifier = 0.12f;


    private void FixedUpdate()
    {
        transform.Translate(transform.forward * ((GameManager.Instance.player.currentSpeed * speedModifier) + 0.01f) * speed * Time.deltaTime);
        //transform.Translate( (transform.forward * speed * Time.deltaTime) + (transform.forward * GameManager.Instance.player.currentSpeed * Time.deltaTime) );
    }
    void Update()
    {


        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
            Destroy(gameObject);
    }

}
