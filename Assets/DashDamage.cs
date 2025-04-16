using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDamage : MonoBehaviour
{
    private AugmentsScript AugmentsScript;
    private PlayerDodge PlayerDodge;

    private void Start()
    {
        AugmentsScript = FindAnyObjectByType<AugmentsScript>();
        PlayerDodge = FindAnyObjectByType<PlayerDodge>();
    }

    private void FixedUpdate()
    {
        if(AugmentsScript.EquipedAugments[10] && ((transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("roll") && transform.parent.GetComponent<PlayerJumpV3>().grounded) || PlayerDodge.airdodgelengthcnt > 0))
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<EnemyHP>() != null)
        {
            collision.GetComponent<EnemyHP>().TakeDamage((int)AugmentsScript.EquipedStats.Damage);
        }
        
    }
}
