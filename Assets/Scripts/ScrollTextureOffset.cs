using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTextureOffset : MonoBehaviour
{
    public Renderer rend;
    //public float scrollOffset;
    private Vector2 scrollOffset = new Vector2(0, 0);
    //public float speed = 1f;
    public Vector2 speed = new Vector2(0, 1);

    // Update is called once per frame
    void Update()
    {
        //scrollOffset += Time.deltaTime * speed;
        scrollOffset = new Vector2(scrollOffset.x + (speed.x * Time.deltaTime), scrollOffset.y + (speed.y * Time.deltaTime));

        //rend.material.SetTextureOffset("_MainTex", new Vector2(0, scrollOffset));
        rend.material.SetTextureOffset("_MainTex", scrollOffset);
    }
}
