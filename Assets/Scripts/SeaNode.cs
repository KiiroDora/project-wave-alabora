using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeaNode : MonoBehaviour
{
    Rigidbody2D rb2d;
    public List<SeaNode> neighborNodes = new();


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void WaveTest()
    {
        rb2d.AddForceY(5f);
    }
}
