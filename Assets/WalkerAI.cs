using System.Collections.Generic;
using UnityEngine;

public class WalkerAI : MonoBehaviour
{
    private GameObject player;
    public float timeunabletomove;
    public int timeunabletomovecounter;
    public GameObject Exclamationmark;
    public float movespeed;

    private float destination;

    private int lasthp;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (FindAnyObjectByType<Global>().atsavepoint)
        {
            return;
        }
        if (collision.transform.GetComponent<PlayerHP>() != null && (transform.position.y - GetComponent<BoxCollider2D>().size.y * transform.localScale.y / 2f >= collision.transform.position.y + collision.transform.GetComponent<BoxCollider2D>().size.y * collision.transform.transform.localScale.y / 2f))
        {
            int direction = (int)((collision.transform.position.x - transform.position.x) / Mathf.Abs(collision.transform.position.x - transform.position.x));
            collision.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 5, 3), ForceMode2D.Impulse);
        }
    }

    private void Start()
    {
        player = FindAnyObjectByType<PlayerHP>().gameObject;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (FindAnyObjectByType<Global>().atsavepoint)
        {
            return;
        }

        Exclamationmark.SetActive(false);
        if (GetComponent<EnemyHP>().enemyNRG > 0)
        {
            ManageMovement();
            managehitstun();

        }



    }


    private void managehitstun()
    {

        if (timeunabletomovecounter > 0)
        {
            timeunabletomovecounter--;
        }

        if (lasthp > GetComponent<EnemyHP>().enemyhp && timeunabletomovecounter == 0)
        {
            timeunabletomovecounter = (int)(timeunabletomove / Time.deltaTime);
        }


        lasthp = GetComponent<EnemyHP>().enemyhp;
    }

    private void ManageMovement()
    {
        Debug.Log(destination);
        if (destination != 0 && timeunabletomovecounter == 0)
        {
            GetComponent<Rigidbody2D>().velocityX = (destination - transform.position.x) / Mathf.Abs(destination - transform.position.x) * movespeed;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocityX = GetComponent<Rigidbody2D>().velocityX * 0.9991f;
        }
        if (Mathf.Abs(destination - transform.position.x) <= 1 || destination == 0)
        {
            if (Random.Range(0, 60) == 25)
            {
                destination = transform.position.x + Random.Range(-5f, 5f);
            }
            else
            {
                destination = 0;
            }
        }
        if (GetComponent<Rigidbody2D>().velocityX < -0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (GetComponent<Rigidbody2D>().velocityX > 0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocityX));
    }
}
