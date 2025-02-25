using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{

    private Animator anim;

    public float dodgecd;
    private int dodgecdcnt;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //dodge cooldown
        if (dodgecdcnt > 0)
        {
            dodgecdcnt -= 1;
        }
    }

    void OnDodge()
    {
        if(FindAnyObjectByType<Global>())
        {
            if(FindAnyObjectByType<Global>().closedmenu)
            {
                FindAnyObjectByType<Global>().closedmenu = false;
                return;
            }
        }
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("roll") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && dodgecdcnt == 0)
        {
            anim.SetTrigger("dodge");
            dodgecdcnt = (int)(dodgecd / Time.fixedDeltaTime);
        }

    }
}
