using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifeTime = 5f;
    public float speed = 4f;
    public bool piercing = false;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Obstacle"))
        {
            var script = other.GetComponent<Obstacle>();

            if (script.obstacleType == Enums.ObstacleType.Enemies || script.obstacleType == Enums.ObstacleType.Rock)
            {
                //Destroy(other.gameObject);
                script.Explode();
                Stats.arrowsHit++;

                if (piercing == false)
                {
                    Destroy(gameObject);
                }
            }

            else if (script.obstacleType == Enums.ObstacleType.Web && piercing == false)
            {
                script.Explode();
                // Destroy(gameObject);
            }
                
        }
    }
}
