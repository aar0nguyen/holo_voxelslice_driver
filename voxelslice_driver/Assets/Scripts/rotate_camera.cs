using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_camera : MonoBehaviour
{
    public float rpm;


    private void Update()
    {
        transform.Rotate(0, rpm * 6f * Time.deltaTime, 0);
    }
}