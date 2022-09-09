using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    public Vector3 speed;
    public Vector3 distance;

    public Vector3 basePosition;

    void Start()
    {
        basePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time;
        Vector3 s = new Vector3(Mathf.Sin(time * speed.x) * distance.x, Mathf.Sin(time * speed.y) * distance.y, Mathf.Sin(time * speed.z) * distance.z);

        transform.position = basePosition + s;
    }
}
