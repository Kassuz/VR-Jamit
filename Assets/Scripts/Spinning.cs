//  By Tommi T
// 
//  Rotates the cogs around the scene

using UnityEngine;
using System.Collections;

public class Spinning : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
