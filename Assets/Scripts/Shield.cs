using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private bool shieldHitOnce = false;

    private void OnEnable()
    {
        shieldHitOnce = false;
    }

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
                
                //PERK
                if (script.obstacleType == Enums.ObstacleType.Log && PlayerStats.shieldLogToArrow == true)
                {
                    GameManager.Instance.player.arrowCount++;
                    GameManager.Instance.GuiManager.UpdateArrows(GameManager.Instance.player.arrowCount);
                }
            }
          
            //PERK
            if (PlayerStats.shieldReduceOtherCD && shieldHitOnce == false)
            {
                shieldHitOnce = true;

                var player = GameManager.Instance.player;
                player.arrowCurrentCD -= 0.5f;
                player.slashCurrentCD -= 0.5f;

                player.UpdateProgressGUI();
            }

        }
    }
}
