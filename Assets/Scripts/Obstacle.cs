using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Obstacle : MonoBehaviour
{
    //public enum ObstacleType
    //{
    //    Web,
    //    Enemies,
    //    Rock,
    //    Hole
    //}

    public ObstacleType obstacleType;
    public GameObject particle;

    public void Explode()
    {
        Destroy(gameObject);
        Instantiate(particle, transform.position, transform.rotation);
    }
}
