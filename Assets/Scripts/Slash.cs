using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public float lifeTime = 0.5f;
    public GameObject hitbox;
    private float currentLifeTime;
    private const float SCALE_HITBOX = 0.0001633931f;

    // Update is called once per frame
    void Update()
    {

        currentLifeTime -= Time.deltaTime;

        if (currentLifeTime <= 0)
            hitbox.SetActive(false);
    }

    public void Activate()
    {
        float scaling = 0;

        if (PlayerStats.swordSizeIncrease == true && GameManager.Instance.player.yVelocity != 0)
            scaling = 15f;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, SCALE_HITBOX + (SCALE_HITBOX * scaling));
        hitbox.SetActive(true);
        currentLifeTime = lifeTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Obstacle"))
        {
            var script = other.GetComponent<Obstacle>();

            if (script.obstacleType == Enums.ObstacleType.Enemies || script.obstacleType == Enums.ObstacleType.Web)
            {
                Telemetry.slashHit++;
                script.Explode();
                //Destroy(other.gameObject);
            }

        }
    }
}
