using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Obstacle"))
        {
            var script = other.GetComponent<Obstacle>();

            if (script.obstacleType == Enums.ObstacleType.Enemies ||
                script.obstacleType == Enums.ObstacleType.Web ||
                script.obstacleType == Enums.ObstacleType.Rock ||
                script.obstacleType == Enums.ObstacleType.Log)
            {
                Telemetry.shieldHit++;
                script.Explode();
                //Destroy(other.gameObject);
            }
          

        }
    }
}
