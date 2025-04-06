using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class passthoughplatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.GetComponent<PlayerHP>() != null)
        {
            if (collision.contacts[0].point.y > collision.transform.GetChild(0).position.y)
            {
                transform.gameObject.layer = LayerMask.NameToLayer("ground");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<PlayerHP>() != null)
        {
            transform.gameObject.layer = LayerMask.NameToLayer("passthroughplatform");
        }
    }
}
