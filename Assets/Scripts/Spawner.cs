using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float minTimer = 5f;
    public float maxTimer = 15f;
    private float timer;

    public List<GameObject> spawnOBJ = new List<GameObject>();

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
            SpawnObject();
            timer = Random.Range(minTimer, maxTimer);
        }
    }

    private void SpawnObject()
    {
        int lane = Random.Range(-1, 2);
        int rdmObj = Random.Range(0, spawnOBJ.Count);

        Vector3 newPos = new Vector3(transform.position.x + lane, transform.position.y, transform.position.z);
        Instantiate(spawnOBJ[rdmObj], newPos, transform.rotation);
    }
}
