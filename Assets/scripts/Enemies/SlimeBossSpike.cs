using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossSpike : MonoBehaviour
{

    public int damage;
    private Animator Animator;
    public float timetofade;
    private int counter=-1;
    public Vector2 pushforce;
    
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AnimatorClipInfo[] animationClip = Animator.GetCurrentAnimatorClipInfo(0);
        int currentFrame = (int)(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (animationClip[0].clip.length * animationClip[0].clip.frameRate));
        if (currentFrame >= 14)
        {
            if(counter == -1)
            {
                counter = (int)(timetofade/Time.deltaTime)+ (int)((timetofade / Time.deltaTime)*0.5f);
            }
            if(counter <= (timetofade / Time.deltaTime))
            {
                float newalpha = Mathf.Clamp01((float)counter / (timetofade / Time.deltaTime));
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, newalpha);
            }
            

            counter--;
            if(counter == 0)
            {

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerHP>().TakeDamage(damage, Vector2.zero, pushforce);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerHP>().TakeDamage(damage, Vector2.zero, pushforce);
        }
    }
}
