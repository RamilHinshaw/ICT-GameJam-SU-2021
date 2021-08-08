using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifeTime = 5f;
    public float speed = 4f;
    public int piercing = 0;

    // Update is called once per frame
    private void Start()
    {
        piercing = PlayerStats.arrowPiercing;
    }

    private void OnEnable()
    {
        piercing = PlayerStats.arrowPiercing;
    }

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
                Telemetry.arrowsHit++;

                if (piercing <= 0)
                {
                    Destroy(gameObject);
                }

                piercing--;
            }

            //else if (script.obstacleType == Enums.ObstacleType.Web && piercing > 0)
            //{
            //    piercing--;
            //    script.Explode();
            //    // Destroy(gameObject);
            //}

            else
            {
                Destroy(gameObject);
            }
                
        }
    }
}
