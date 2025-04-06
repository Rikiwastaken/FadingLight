using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public GameObject projectile;
    public float duration;
    public int damage;
    public int energydamage;
    public float rateoffire;
    private int durationcounter;
    private int rateoffirecounter;

    private bool touchedground;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("ground") && !touchedground)
        {
            touchedground = true;
            durationcounter = (int)(duration/Time.deltaTime);
            GetComponent<Animator>().speed = 1.0f;
            rateoffirecounter = (int)(rateoffire / Time.deltaTime)*2;
            GetComponent<Rigidbody2D>().simulated = false;
        }
    }

    private void Start()
    {
        GetComponent<Animator>().speed = 0f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(rateoffirecounter==0)
        {
            GameObject newproj =Instantiate(projectile, transform.GetChild(0).position, Quaternion.identity);
            newproj.transform.localScale = Vector3.one*0.5f; 
            newproj.GetComponent<BulletScript>().damage = damage;
            newproj.GetComponent<BulletScript>().EnergyDamage = energydamage;
            newproj.GetComponent<BulletScript>().direction = (int)(transform.localScale.x/Mathf.Abs(transform.localScale.x));
            rateoffirecounter=(int)(rateoffire/Time.deltaTime);
        }
        else
        {
            rateoffirecounter--;
        }
        if(durationcounter==0 && touchedground)
        {
            Destroy(gameObject);
        }
        else if (durationcounter > 0)
        {
            durationcounter--;
        }
    }
}
