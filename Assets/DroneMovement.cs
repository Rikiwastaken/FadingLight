using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{

    public float movespeed;
    private Vector3 offset;
    Transform player;
    public float maxspeed;
    private int framecd;
    private Vector2 rd;

    private void Start()
    {
        player = GameObject.FindAnyObjectByType<AugmentsScript>().transform;
        offset = transform.position-transform.parent.position;
        transform.SetParent(null, true);
        rd = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector2 returnpos = player.position + offset-transform.position;
        GetComponent<Rigidbody2D>().velocity = returnpos.normalized * maxspeed * Vector2.Distance(player.position + offset, transform.position)+rd;
        if(framecd==0)
        {
            framecd = (int)(1/Time.deltaTime);
            rd = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        }
        else
        {
            framecd--;
        }
        //if (Vector2.Distance(player.position + offset, transform.position) > 0.1f && Vector2.Distance(player.position + offset,transform.position)<2f)
        //{
        //    if (GetComponent<Rigidbody2D>().velocity.magnitude < maxspeed )
        //    {
        //        GetComponent<Rigidbody2D>().velocity += returnpos.normalized * movespeed ;
        //    }
        //    else
        //    {
        //        GetComponent<Rigidbody2D>().velocity = returnpos.normalized * maxspeed;
        //    }
        //}
        //else if(Vector2.Distance(player.position + offset, transform.position) >= 1)
        //{
        //    if (GetComponent<Rigidbody2D>().velocity.magnitude < maxspeed * Vector2.Distance(player.position + offset, transform.position))
        //    {
        //        GetComponent<Rigidbody2D>().velocity += returnpos.normalized * movespeed * Vector2.Distance(player.position + offset, transform.position);
        //    }
        //    else
        //    {
        //        GetComponent<Rigidbody2D>().velocity = returnpos.normalized * maxspeed * Vector2.Distance(player.position + offset, transform.position);
        //    }
        //}
        //else
        //{
        //    GetComponent<Rigidbody2D>().velocity *= 0.9f;
        //}
    }
}
