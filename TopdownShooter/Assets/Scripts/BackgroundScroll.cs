using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    private Vector2 offset;
    public float xVel, yVel;
    private Material material;

    void Start()
    {
        material = GetComponent<Renderer>().material;    
    }

    void Update()
    {
        offset = new Vector2(xVel, yVel);
        material.mainTextureOffset += offset * Time.deltaTime;
    }
}
