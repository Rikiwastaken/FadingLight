using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossSpike : MonoBehaviour
{

    public int damage;
    private Animator Animator;
    
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AnimatorClipInfo[] animationClip = Animator.GetCurrentAnimatorClipInfo(0);
        int currentFrame = (int)(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (animationClip[0].clip.length * animationClip[0].clip.frameRate));
        if (currentFrame >= 16)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, GetComponent<SpriteRenderer>().color.a - 0.05f);
        }

        if (GetComponent<SpriteRenderer>().color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        int currentFrame = (int)(Animator.GetCurrentAnimatorClipInfo(0)[0].weight * (Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * Animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate));
        if (collision.transform.tag == "Player" && currentFrame >= 12)
        {
            collision.transform.GetComponent<PlayerHP>().TakeDamage(damage, new Vector2(collision.transform.GetComponent<Rigidbody2D>().velocityX, collision.transform.GetComponent<PlayerHP>().hitjumpforce), Vector2.zero);
        }
    }
}
