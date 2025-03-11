using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossSpike : MonoBehaviour
{

    public int damage;
    private Animator Animator;
    public float timetofade;
    private int counter=-1;
    
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AnimatorClipInfo[] animationClip = Animator.GetCurrentAnimatorClipInfo(0);
        int currentFrame = (int)(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (animationClip[0].clip.length * animationClip[0].clip.frameRate));
        if (currentFrame >= 13)
        {
            Animator.speed = 0;
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
        if(Animator==null)
        {
            Animator = GetComponent<Animator>();
        }
        AnimatorClipInfo[] animationClip = Animator.GetCurrentAnimatorClipInfo(0);
        int currentFrame = (int)(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (animationClip[0].clip.length * animationClip[0].clip.frameRate));
        if (collision.transform.tag == "Player" && (counter > (timetofade / Time.deltaTime) || GetComponent<SpriteRenderer>().color.a>0.75f))
        {
            collision.transform.GetComponent<PlayerHP>().TakeDamage(damage, new Vector2(collision.transform.GetComponent<Rigidbody2D>().velocityX, collision.transform.GetComponent<PlayerHP>().hitjumpforce), Vector2.zero);
        }
    }
}
