using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTextureOffset : MonoBehaviour
{
    public Renderer rend;
    public float scrollOffset;
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        scrollOffset += Time.deltaTime * speed;

        rend.material.SetTextureOffset("_MainTex", new Vector2(0, scrollOffset));
    }
}
