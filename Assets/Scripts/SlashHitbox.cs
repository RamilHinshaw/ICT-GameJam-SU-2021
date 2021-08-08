using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHitbox : MonoBehaviour
{
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
