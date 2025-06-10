using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingContaminatedAI : MonoBehaviour
{
    
    public float damage;
    private GameObject player;
    public float mindist;

    private int lasthp;
    public float timeunabletomove;
    public int timeunabletomovecounter;

    public GameObject ProjectilePrefab;
    public Transform wheretoplacePrefabs;
    public bool allowexplosion;

    private bool exploded;

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

        if (GetComponent<EnemyHP>().enemyNRG > 0)
        {
            ManageAttack();

            managehitstun();
        }

        ManageExplosion();

    }

    private void ManageAttack()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) <= mindist && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ExplosiveContaminatedExplosion") && timeunabletomovecounter ==0 && allowexplosion)
        {
            GetComponent<Animator>().SetTrigger("Explosion");
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

    private void ManageExplosion()
    {
        if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ExplosiveContaminatedDeath")&& !exploded)
        {
            exploded = true;
            for (int i = 0;i < wheretoplacePrefabs.childCount;i++)
            {
                GameObject newproj = Instantiate(ProjectilePrefab, wheretoplacePrefabs.GetChild(i).transform.position, Quaternion.identity);
                newproj.GetComponent<BulletScript>().directionvector = wheretoplacePrefabs.GetChild(i).transform.position - transform.position;
                newproj.GetComponent<BulletScript>().sender = gameObject;
                newproj.GetComponent<BulletScript>().damagePlayer = true;
                newproj.GetComponent<BulletScript>().damage = (int)damage;
                newproj.transform.localScale = Vector3.one;
            }
        }
        if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ExplosiveContaminatedDeath"))
        {
            GetComponent<EnemyHP>().enemyhp = 0;
        }
    }
}
