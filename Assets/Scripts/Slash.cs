using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public float lifeTime = 0.5f;
    private float currentLifeTime;

    // Update is called once per frame
    void Update()
    {

        currentLifeTime -= Time.deltaTime;

        if (currentLifeTime <= 0)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        currentLifeTime = lifeTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Obstacle"))
        {
            var script = other.GetComponent<Obstacle>();

            if (script.obstacleType == Enums.ObstacleType.Enemies || script.obstacleType == Enums.ObstacleType.Web)
            {
                Stats.slashHit++;
                Destroy(other.gameObject);
            }

        }
    }
}
