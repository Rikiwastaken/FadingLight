using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveScript : MonoBehaviour
{

    public float forcestr;
    private Animator animator;
    private AnimatorStateInfo animStateInfo;
    private float NTime;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        NTime = animStateInfo.normalizedTime;

        if (NTime > 1.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHP enemy = collision.gameObject.GetComponent<EnemyHP>();
        if(enemy!=null)
        {
            if(!enemy.isbig && !enemy.isboss)
            {
                Vector2 forcetoapply = (enemy.transform.position-transform.position).normalized*forcestr;
                enemy.transform.GetComponent<Rigidbody2D>().AddForce(forcetoapply,ForceMode2D.Impulse);
            }
        }
    }
}
