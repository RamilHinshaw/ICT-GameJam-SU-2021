using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float timer = 1f;
    // Start is called before the first frame update
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
            Destroy(gameObject);
    }

 
}
