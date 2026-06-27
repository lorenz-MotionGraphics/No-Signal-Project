using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float speed = 2.0f;    // How fast it moves
    public float height = 0.5f;   // How far up/down it goes
    
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    void Start() {
        // Store the starting position of the object
        posOffset = transform.position;
    }

    void Update() {
        tempPos = posOffset;
        // Use Sin to calculate the new Y position
        tempPos.y += Mathf.Sin(Time.fixedTime * speed) * height;

        transform.position = tempPos;
    }
}