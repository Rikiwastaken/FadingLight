using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Vector2 offset;
    private Transform Player;
    
    void Start()
    {
        Player = FindAnyObjectByType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(offset.x+Player.position.x, offset.y + Player.position.y, transform.position.z);
    }
}
