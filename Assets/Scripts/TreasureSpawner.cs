using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureSpawner : MonoBehaviour
{
    public float minTimer= 5f;
    public float maxTimer = 15f;
    private float timer;

    public GameObject treasureOBJ;

    private void Start()
    {
        timer = Random.Range(minTimer, maxTimer);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnTreasure();
            timer = Random.Range(minTimer, maxTimer);
        }
    }

    private void SpawnTreasure()
    {
        int lane = Random.Range(-1, 2);

        Vector3 newPos = new Vector3(transform.position.x + lane, transform.position.y, transform.position.z);
        Instantiate(treasureOBJ, newPos, transform.rotation);
    }
}
